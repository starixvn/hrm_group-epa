using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GO.UserControl.Client.Core.Services;
using GO.UserControl.Client.Core.Session;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.Util;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Enumerations;
using SMS.EPA.Criteria.Discipline;
using SMS.EPA.Criteria.Employee;
using SMS.EPA.Criteria.EmployeeEPA;
using SMS.EPA.Criteria.Period;
using SMS.EPA.Criteria.Report;
using SMS.EPA.Criteria.TargetEmployeeSetting;
using SMS.EPA.Models.Enitites;
using SMS.EPA.Models.Report;
using SMS.EPA.Models.Report.EPASummary;
using SMS.EPA.Resources;
using SMS.EPA.Services.Interfaces;
using SMS.EPA.Services.Interfaces.ExportFile;

namespace SMS.EPA.Site.Controllers.Export;

#pragma warning disable S107 // Methods should not have too many parameters
public class ExportReportController(
    IReportService reportService,
    IEpaDetailReportService epaDetailReportService,
    IDisciplineService disciplineService,
    IEmployeeEPAService employeeEPAService,
    IExportExcelService exportExcelService,
    ITargetEmployeeSettingService targetEmployeeSettingService,
    IPeriodService periodService,
    IEmployeeService employeeService,
    IPersonalFlowService personalFlowService,
    IUserControlSession userControlSession,
    IRoleService roleService,
    IDepartmentFlowService departmentFlowService,
    IWebHostEnvironment webHostEnvironment) : Controller
#pragma warning disable S107 // Methods should not have too many parameters
{
    private const int DefaultContentRowStartIndexForTemplate = 3;

    private const string DisciplineSummaryByPeriodTemplatePath = "ExcelTemplates/DisciplineSummaryByPeriodReport.xls";
    private const string DisciplineSummaryTemplatePath = "ExcelTemplates/DisciplineSummaryReport.xls";
    private const string EmployeeBasicInfoTemplate = "ExcelTemplates/EmployeeBasicInfoTemplate.xls";
    private const string EmployeeFlowSettingTemplate = "ExcelTemplates/EmployeeFlowSettingTemplate.xls";
    private const string GroupFlowSettingTemplate = "ExcelTemplates/GroupFlowSettingTemplate.xlsx";
    private const string PersonalFlowSettingTemplate = "ExcelTemplates/PersonalFlowSettingTemplate.xlsx";
    private const string TargetPersonnelSettingTemplate = "ExcelTemplates/TargetPersonnelSettingTemplate.xls";
    private const string UpdateFinalScoreTemplatePath = "ExcelTemplates/UpdateFinalScore.xls";

    public async Task<ActionResult> ExportDisciplineSummaryForOtherSystemReport(GetDisciplineSummaryForOtherSystemCriteria criteria)
    {
        const int contentFirstRowIndex = 3;
        if (criteria != null)
        {
            var employeeDisciplines = disciplineService.GetDisciplineSummaryForOtherSystem(criteria);

            if (employeeDisciplines != null && employeeDisciplines.Any())
            {
                exportExcelService.FileName = "Discipline_Summary_Report";
                exportExcelService.SheetName = "Discipline Summary";
                exportExcelService.Title = "Discipline Summary Report";

                exportExcelService.InitializeWithTemplate(Path.Combine(webHostEnvironment.ContentRootPath, DisciplineSummaryByPeriodTemplatePath));

                exportExcelService.ContentRowStartIndex = contentFirstRowIndex;
                CreateRedStyleForDisciplineSummaryByPeriodReport();

                await exportExcelService.ExportWithTemplate(employeeDisciplines, true, true);
            }
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportDisciplineSummaryReport(GetDisciplineSummaryCriteria criteria)
    {
        const int contentFirstRowIndex = 3;
        if (criteria != null)
        {
            criteria.ApproverId = userControlSession.Get<int>(Constants.CurrentEmployeeIdKey);
            var employeeDisciplines = disciplineService.GetDisciplineSummary(criteria);

            if (employeeDisciplines != null && employeeDisciplines.Any())
            {
                exportExcelService.FileName = Lang.AttendanceSummaryReportName;
                exportExcelService.SheetName = Lang.AttendanceSummaryReportSheet;
                exportExcelService.Title = GetDisciplineSummaryReportTitle(criteria);

                exportExcelService.InitializeWithTemplate(Path.Combine(webHostEnvironment.ContentRootPath, DisciplineSummaryTemplatePath));
                exportExcelService.ContentRowStartIndex = contentFirstRowIndex;

                CreateRedStyleForDisciplineSummaryReport();

                await exportExcelService.ExportWithTemplate(employeeDisciplines, true, true);
            }
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportPeriodEmployeeSetting(GetPeriodEmployeeSettingCriteria criteria)
    {
        var employeeFlowSettings = periodService.GetPeriodEmployeeSettingById(criteria);

        exportExcelService.FileName = string.Format(Lang.EmployeeFlowSettingReportName, criteria.PeriodName);
        exportExcelService.SheetName = Lang.EmployeeFlowSettingReportSheet;
        exportExcelService.Title = string.Format(Lang.EmployeeFlowSettingReportTitle, criteria.PeriodName);
        exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

        exportExcelService.InitializeWithTemplate(Path.Combine(webHostEnvironment.ContentRootPath, EmployeeFlowSettingTemplate));

        await exportExcelService.ExportWithTemplate(employeeFlowSettings, true, true);

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportEmployeeInfoReport(GetEmployeeCriteria criteria)
    {
        if (criteria != null)
        {
            var roles = roleService.GetBySiteId(userControlSession.SiteId);
            IEnumerable<Employee> employees = employeeService.GetEmployee(criteria, roles);

            if (employees.Any())
            {
                exportExcelService.FileName = "Employee_Infomation";
                exportExcelService.SheetName = "Employee Infomation";

                exportExcelService.InitializeWithTemplate(Path.Combine(webHostEnvironment.ContentRootPath, EmployeeBasicInfoTemplate));

                exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

                await exportExcelService.ExportWithTemplate(employees, true);
            }
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportEPAAfterUpdateFinalScore(GetEPAToUpdateFinalScoreCriteria criteria)
    {
        if (criteria != null)
        {
            var epaInfos = employeeEPAService.GetEPAToUpdateFinalScore(criteria);
            var employeeIds = criteria.EmployeeIds.Split(',');
            epaInfos = epaInfos.Join(employeeIds, x => x.Employee.EmployeeId.ToString(), y => y, (x, _) => x)
                .ToList();

            if (epaInfos.Any())
            {
                exportExcelService.FileName = "Update_Final_Score";
                exportExcelService.SheetName = "Update Final Score";
                exportExcelService.Title = criteria.PeriodName;

                exportExcelService.InitializeWithTemplate(Path.Combine(webHostEnvironment.ContentRootPath, UpdateFinalScoreTemplatePath));

                exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

                await exportExcelService.ExportWithTemplate(epaInfos, true, true);
            }
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportEPADetailReport(GetEPADetailCriteria criteria)
    {
        criteria.ActorId = userControlSession.Get<int>(Constants.CurrentEmployeeIdKey);

        var roles = roleService.GetBySiteId(userControlSession.SiteId);
        var epaDetail = reportService.GetEPADetailReport(criteria, criteria.ActorId, roles);

        epaDetail.Period = reportService.GetEPADetailPeriodForExportedFileName(criteria.PeriodId?.ToString());

        if (string.IsNullOrEmpty(criteria.SortColumnName) && epaDetail.EpaInfos.Any())
        {
            epaDetail.EpaInfos = epaDetail.EpaInfos.First().Period.TargetTypeId == (byte)TargetType.Probation
                ? epaDetail.EpaInfos.OrderByDescending(x => x.Employee.Code)
                : epaDetail.EpaInfos.OrderBy(x => x.Employee.GroupCode).ThenBy(x => x.Employee.Code);
        }

        epaDetailReportService.Render(epaDetail);

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportEPASummaryReport(GetEPASummaryCriteria criteria)
    {
        criteria.ActorId = userControlSession.Get<int>(Constants.CurrentEmployeeIdKey);

        var epaSummary = reportService.GetEPASummaryReport(criteria);
        epaSummary.Period = reportService.GetEPADetailPeriodForExportedFileName(criteria.PeriodIds);

        var output = reportService.ExportEPASummaryReport(epaSummary);

        var fileName = GetEPASummaryReportFilename(epaSummary);

        return File(output.ToArray(), "application/vnd.ms-excel", fileName);
    }

    public async Task<ActionResult> ExportMonthlyEPADetailReport(GetMonthlyEPADetailCriteria criteria)
    {
        criteria.EmployeeId = userControlSession.Get<int>(Constants.CurrentEmployeeIdKey);

        var roles = roleService.GetBySiteId(userControlSession.SiteId);
        var epaDetail = reportService.GetMonthlyEPADetailReport(criteria, criteria.EmployeeId, roles);
        epaDetail.Period = reportService.GetEPADetailPeriodForExportedFileName(criteria.PeriodId?.ToString());
        epaDetailReportService.Render(epaDetail);

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportMonthlyEPASummaryReport(GetMonthlyEPASummaryCriteria criteria)
    {
        var fileName = Lang.MonthlyEPASummaryReportName + ".xls";

        criteria.EmployeeId = userControlSession.Get<int>(Constants.CurrentEmployeeIdKey);

        var epaSummary = reportService.GetMonthlyEPASummaryReport(criteria);

        var output = reportService.ExportMonthlyEPASummaryReport(epaSummary);

        return File(output.ToArray(), "application/vnd.ms-excel", fileName);
    }

    public async Task<ActionResult> ExportPersonalFlowSetting(int flowTemplateId, string flowTemplateName)
    {
        if (flowTemplateId > 0 && !string.IsNullOrEmpty(flowTemplateName))
        {
            var flowSettings = personalFlowService.GetByFlowTemplateId(flowTemplateId);

            exportExcelService.FileName = "Personal_Flow_Setting";
            exportExcelService.SheetName = "Personal Flow";
            exportExcelService.Title = $"{flowTemplateName} - Personal Flow Setting";

            exportExcelService.InitializeWithTemplate(Path.Combine(webHostEnvironment.ContentRootPath, PersonalFlowSettingTemplate));

            exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

            await exportExcelService.ExportWithTemplate(flowSettings, true, true);
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportGroupFlowSetting(int flowTemplateId, string flowTemplateName)
    {
        if (flowTemplateId > 0 && !string.IsNullOrEmpty(flowTemplateName))
        {
            var flowSettings = departmentFlowService.GetByFlowTemplateId(flowTemplateId);

            exportExcelService.FileName = "Department_Flow_Setting";
            exportExcelService.SheetName = "Department Flow";
            exportExcelService.Title = $"{flowTemplateName} - Department Flow Setting";

            exportExcelService.InitializeWithTemplate(Path.Combine(webHostEnvironment.ContentRootPath, GroupFlowSettingTemplate));

            exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

            await exportExcelService.ExportWithTemplate(flowSettings, true, true);
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportTargetPersonnelSettingReport(GetAllTargetEmployeeSettingCriteria criteria)
    {
        if (criteria != null)
        {
            var data = targetEmployeeSettingService.GetTargetEmployeeSetting(criteria);

            exportExcelService.FileName = "Target_Personnel_Setting";
            exportExcelService.SheetName = "Target Personnel Setting";
            exportExcelService.Title = "Target Personnel Setting";

            exportExcelService.InitializeWithTemplate(Path.Combine(webHostEnvironment.ContentRootPath, TargetPersonnelSettingTemplate));
            exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

            await exportExcelService.ExportWithTemplate(data, true);
        }

        return new EmptyResult();
    }

    private static ConditionalCellStyle CreateCellStyleIfLessThanZero(int styleIndex, int columnIndex)
    {
        return new ConditionalCellStyle
        {
            ColumnIndex = columnIndex,
            Condition = currentObject =>
            {
                var value = currentObject.GetType().GetProperties()[columnIndex].GetValue(currentObject, null);
                if (!float.TryParse(value.ToString(), out var number))
                {
                    return false;
                }

                return number < 0;
            },
            StyleIndex = styleIndex
        };
    }

    private static string GetDisciplineSummaryReportTitle(GetDisciplineSummaryCriteria criteria)
    {
        var startDate = criteria.StartDate.ToString(Constants.DateFormat);
        var endDate = criteria.EndDate.ToString(Constants.DateFormat);

        return string.Format(Lang.AttendanceSummaryReportTitleNoPrefix, startDate, endDate);
    }

    private static string GetEPASummaryReportFilename(EPASummaryModel epaSummary)
    {
        var fileName = $"{Lang.EPASummaryReportName}.xls";

        if (!string.IsNullOrEmpty(epaSummary.EpaYear) && !string.IsNullOrEmpty(epaSummary.EpaEvaluationPeriod))
        {
            fileName = $"{epaSummary.EpaYear}_{epaSummary.EpaEvaluationPeriod}_{Lang.EPASummaryReportName}.xls";
        }

        return fileName;
    }

    private void CreateRedStyleForDisciplineSummaryByPeriodReport()
    {
        CreateRedStyleForDisciplineSummaryReport();
    }

    private void CreateRedStyleForDisciplineSummaryReport()
    {
        const int lastCollumnIndex = 20;
        // Style
        var redStyleIndex = exportExcelService.CreateNewColumnStyle();
        var style = exportExcelService.ColumnsStyles[redStyleIndex];
        var redFont = exportExcelService.Workbook.CreateFont();
        redFont.Color = HSSFColor.Red.Index;
        style.SetFont(redFont);

        for (var i = 8; i <= lastCollumnIndex; i++)
        {
            var redStyle = CreateCellStyleIfLessThanZero(redStyleIndex, i);
            exportExcelService.ConditionalCellStyles.Add(redStyle);
        }
    }
}