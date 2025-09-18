using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GO.UserControl.Client.Core.Services;
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

namespace SMS.EPA.Site.Controllers.Export;

public class ExportReportController(
    IReportFacadeService reportFacadeService,
    IRoleService roleService,
    IPeriodService periodService,
    IEmployeeService employeeService,
    IPersonalFlowService personalFlowService,
    IDepartmentFlowService departmentFlowService,
    IHostFacadeService hostFacadeService) : Controller
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
            var employeeDisciplines = reportFacadeService.DisciplineService.GetDisciplineSummaryForOtherSystem(criteria);

            if (employeeDisciplines != null && employeeDisciplines.Any())
            {
                reportFacadeService.ExportExcelService.FileName = "Discipline_Summary_Report";
                reportFacadeService.ExportExcelService.SheetName = "Discipline Summary";
                reportFacadeService.ExportExcelService.Title = "Discipline Summary Report";

                reportFacadeService.ExportExcelService.InitializeWithTemplate(Path.Combine(hostFacadeService.WebHostEnvironment.ContentRootPath, DisciplineSummaryByPeriodTemplatePath));

                reportFacadeService.ExportExcelService.ContentRowStartIndex = contentFirstRowIndex;
                CreateRedStyleForDisciplineSummaryByPeriodReport();

                await reportFacadeService.ExportExcelService.ExportWithTemplate(employeeDisciplines, true, true);
            }
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportDisciplineSummaryReport(GetDisciplineSummaryCriteria criteria)
    {
        const int contentFirstRowIndex = 3;
        if (criteria != null)
        {
            criteria.ApproverId = hostFacadeService.UserControlSession.Get<int>(Constants.CurrentEmployeeIdKey);
            var employeeDisciplines = reportFacadeService.DisciplineService.GetDisciplineSummary(criteria);

            if (employeeDisciplines != null && employeeDisciplines.Any())
            {
                reportFacadeService.ExportExcelService.FileName = Lang.AttendanceSummaryReportName;
                reportFacadeService.ExportExcelService.SheetName = Lang.AttendanceSummaryReportSheet;
                reportFacadeService.ExportExcelService.Title = GetDisciplineSummaryReportTitle(criteria);

                reportFacadeService.ExportExcelService.InitializeWithTemplate(Path.Combine(hostFacadeService.WebHostEnvironment.ContentRootPath, DisciplineSummaryTemplatePath));
                reportFacadeService.ExportExcelService.ContentRowStartIndex = contentFirstRowIndex;

                CreateRedStyleForDisciplineSummaryReport();

                await reportFacadeService.ExportExcelService.ExportWithTemplate(employeeDisciplines, true, true);
            }
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportPeriodEmployeeSetting(GetPeriodEmployeeSettingCriteria criteria)
    {
        var employeeFlowSettings = periodService.GetPeriodEmployeeSettingById(criteria);

        reportFacadeService.ExportExcelService.FileName = string.Format(Lang.EmployeeFlowSettingReportName, criteria.PeriodName);
        reportFacadeService.ExportExcelService.SheetName = Lang.EmployeeFlowSettingReportSheet;
        reportFacadeService.ExportExcelService.Title = string.Format(Lang.EmployeeFlowSettingReportTitle, criteria.PeriodName);
        reportFacadeService.ExportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

        reportFacadeService.ExportExcelService.InitializeWithTemplate(Path.Combine(hostFacadeService.WebHostEnvironment.ContentRootPath, EmployeeFlowSettingTemplate));

        await reportFacadeService.ExportExcelService.ExportWithTemplate(employeeFlowSettings, true, true);

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportEmployeeInfoReport(GetEmployeeCriteria criteria)
    {
        if (criteria != null)
        {
            var roles = roleService.GetBySiteId(hostFacadeService.UserControlSession.SiteId);
            IEnumerable<Employee> employees = employeeService.GetEmployee(criteria, roles);

            if (employees.Any())
            {
                reportFacadeService.ExportExcelService.FileName = "Employee_Infomation";
                reportFacadeService.ExportExcelService.SheetName = "Employee Infomation";

                reportFacadeService.ExportExcelService.InitializeWithTemplate(Path.Combine(hostFacadeService.WebHostEnvironment.ContentRootPath, EmployeeBasicInfoTemplate));

                reportFacadeService.ExportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

                await reportFacadeService.ExportExcelService.ExportWithTemplate(employees, true);
            }
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportEPAAfterUpdateFinalScore(GetEPAToUpdateFinalScoreCriteria criteria)
    {
        if (criteria != null)
        {
            var epaInfos = reportFacadeService.EmployeeEPAService.GetEPAToUpdateFinalScore(criteria);
            var employeeIds = criteria.EmployeeIds.Split(',');
            epaInfos = epaInfos.Join(employeeIds, x => x.Employee.EmployeeId.ToString(), y => y, (x, _) => x)
                .ToList();

            if (epaInfos.Any())
            {
                reportFacadeService.ExportExcelService.FileName = "Update_Final_Score";
                reportFacadeService.ExportExcelService.SheetName = "Update Final Score";
                reportFacadeService.ExportExcelService.Title = criteria.PeriodName;

                reportFacadeService.ExportExcelService.InitializeWithTemplate(Path.Combine(hostFacadeService.WebHostEnvironment.ContentRootPath, UpdateFinalScoreTemplatePath));

                reportFacadeService.ExportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

                await reportFacadeService.ExportExcelService.ExportWithTemplate(epaInfos, true, true);
            }
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportEPADetailReport(GetEPADetailCriteria criteria)
    {
        criteria.ActorId = hostFacadeService.UserControlSession.Get<int>(Constants.CurrentEmployeeIdKey);

        var roles = roleService.GetBySiteId(hostFacadeService.UserControlSession.SiteId);
        var epaDetail = reportFacadeService.ReportService.GetEPADetailReport(criteria, criteria.ActorId, roles);

        epaDetail.Period = reportFacadeService.ReportService.GetEPADetailPeriodForExportedFileName(criteria.PeriodId?.ToString());

        if (string.IsNullOrEmpty(criteria.SortColumnName) && epaDetail.EpaInfos.Any())
        {
            epaDetail.EpaInfos = epaDetail.EpaInfos.First().Period.TargetTypeId == (byte)TargetType.Probation
                ? epaDetail.EpaInfos.OrderByDescending(x => x.Employee.Code)
                : epaDetail.EpaInfos.OrderBy(x => x.Employee.GroupCode).ThenBy(x => x.Employee.Code);
        }

        reportFacadeService.EpaDetailReportService.Render(epaDetail);

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportEPASummaryReport(GetEPASummaryCriteria criteria)
    {
        criteria.ActorId = hostFacadeService.UserControlSession.Get<int>(Constants.CurrentEmployeeIdKey);

        var epaSummary = reportFacadeService.ReportService.GetEPASummaryReport(criteria);
        epaSummary.Period = reportFacadeService.ReportService.GetEPADetailPeriodForExportedFileName(criteria.PeriodIds);

        var output = reportFacadeService.ReportService.ExportEPASummaryReport(epaSummary);

        var fileName = GetEPASummaryReportFilename(epaSummary);

        return File(output.ToArray(), "application/vnd.ms-excel", fileName);
    }

    public async Task<ActionResult> ExportMonthlyEPADetailReport(GetMonthlyEPADetailCriteria criteria)
    {
        criteria.EmployeeId = hostFacadeService.UserControlSession.Get<int>(Constants.CurrentEmployeeIdKey);

        var roles = roleService.GetBySiteId(hostFacadeService.UserControlSession.SiteId);
        var epaDetail = reportFacadeService.ReportService.GetMonthlyEPADetailReport(criteria, criteria.EmployeeId, roles);
        epaDetail.Period = reportFacadeService.ReportService.GetEPADetailPeriodForExportedFileName(criteria.PeriodId?.ToString());
        reportFacadeService.MonthlyEpaDetailReportService.Render(epaDetail);

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportMonthlyEPASummaryReport(GetMonthlyEPASummaryCriteria criteria)
    {
        var fileName = Lang.MonthlyEPASummaryReportName + ".xls";

        criteria.EmployeeId = hostFacadeService.UserControlSession.Get<int>(Constants.CurrentEmployeeIdKey);

        var epaSummary = reportFacadeService.ReportService.GetMonthlyEPASummaryReport(criteria);

        var output = reportFacadeService.ReportService.ExportMonthlyEPASummaryReport(epaSummary);

        return File(output.ToArray(), "application/vnd.ms-excel", fileName);
    }

    public async Task<ActionResult> ExportPersonalFlowSetting(int flowTemplateId, string flowTemplateName)
    {
        if (flowTemplateId > 0 && !string.IsNullOrEmpty(flowTemplateName))
        {
            var flowSettings = personalFlowService.GetByFlowTemplateId(flowTemplateId);

            reportFacadeService.ExportExcelService.FileName = "Personal_Flow_Setting";
            reportFacadeService.ExportExcelService.SheetName = "Personal Flow";
            reportFacadeService.ExportExcelService.Title = $"{flowTemplateName} - Personal Flow Setting";

            reportFacadeService.ExportExcelService.InitializeWithTemplate(Path.Combine(hostFacadeService.WebHostEnvironment.ContentRootPath, PersonalFlowSettingTemplate));

            reportFacadeService.ExportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

            await reportFacadeService.ExportExcelService.ExportWithTemplate(flowSettings, true, true);
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportGroupFlowSetting(int flowTemplateId, string flowTemplateName)
    {
        if (flowTemplateId > 0 && !string.IsNullOrEmpty(flowTemplateName))
        {
            var flowSettings = departmentFlowService.GetByFlowTemplateId(flowTemplateId);

            reportFacadeService.ExportExcelService.FileName = "Department_Flow_Setting";
            reportFacadeService.ExportExcelService.SheetName = "Department Flow";
            reportFacadeService.ExportExcelService.Title = $"{flowTemplateName} - Department Flow Setting";

            reportFacadeService.ExportExcelService.InitializeWithTemplate(Path.Combine(hostFacadeService.WebHostEnvironment.ContentRootPath, GroupFlowSettingTemplate));

            reportFacadeService.ExportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

            await reportFacadeService.ExportExcelService.ExportWithTemplate(flowSettings, true, true);
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportTargetPersonnelSettingReport(GetAllTargetEmployeeSettingCriteria criteria)
    {
        if (criteria != null)
        {
            var data = reportFacadeService.TargetEmployeeSettingService.GetTargetEmployeeSetting(criteria);

            reportFacadeService.ExportExcelService.FileName = "Target_Personnel_Setting";
            reportFacadeService.ExportExcelService.SheetName = "Target Personnel Setting";
            reportFacadeService.ExportExcelService.Title = "Target Personnel Setting";

            reportFacadeService.ExportExcelService.InitializeWithTemplate(Path.Combine(hostFacadeService.WebHostEnvironment.ContentRootPath, TargetPersonnelSettingTemplate));
            reportFacadeService.ExportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

            await reportFacadeService.ExportExcelService.ExportWithTemplate(data, true);
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
        var redStyleIndex = reportFacadeService.ExportExcelService.CreateNewColumnStyle();
        var style = reportFacadeService.ExportExcelService.ColumnsStyles[redStyleIndex];
        var redFont = reportFacadeService.ExportExcelService.Workbook.CreateFont();
        redFont.Color = HSSFColor.Red.Index;
        style.SetFont(redFont);

        for (var i = 8; i <= lastCollumnIndex; i++)
        {
            var redStyle = CreateCellStyleIfLessThanZero(redStyleIndex, i);
            reportFacadeService.ExportExcelService.ConditionalCellStyles.Add(redStyle);
        }
    }
}