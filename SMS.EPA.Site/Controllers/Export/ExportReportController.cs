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
using SMS.EPA.Services.Implement;
using SMS.EPA.Services.Implement.ExportFile;
using SMS.EPA.Services.Interfaces;
using SMS.EPA.Services.Interfaces.ExportFile;

namespace SMS.EPA.Site.Controllers.Export;

public class ExportReportController : Controller
{
    private const int DefaultContentRowStartIndexForTemplate = 3;

    private const string DisciplineSummaryByPeriodTemplatePath = "App_Data/ExcelTemplates/DisciplineSummaryByPeriodReport.xls";
    private const string DisciplineSummaryTemplatePath = "App_Data/ExcelTemplates/DisciplineSummaryReport.xls";
    private const string EmployeeBasicInfoTemplate = "App_Data/ExcelTemplates/EmployeeBasicInfoTemplate.xls";
    private const string EmployeeFlowSettingTemplate = "App_Data/ExcelTemplates/EmployeeFlowSettingTemplate.xls";
    private const string GroupFlowSettingTemplate = "App_Data/ExcelTemplates/GroupFlowSettingTemplate.xlsx";
    private const string PersonalFlowSettingTemplate = "App_Data/ExcelTemplates/PersonalFlowSettingTemplate.xlsx";
    private const string TargetPersonnelSettingTemplate = "App_Data/ExcelTemplates/TargetPersonnelSettingTemplate.xls";
    private const string UpdateFinalScoreTemplatePath = "App_Data/ExcelTemplates/UpdateFinalScore.xls";
    private readonly IDisciplineService _disciplineService;
    private readonly IEmployeeEPAService _employeeEPAService;
    private readonly IEmployeeService _employeeService;
    private readonly IEpaDetailReportService _epaDetailReportService;
    private readonly IExportExcelService _exportExcelService;
    private readonly IEpaDetailReportService _monthlyEpaDetailReportService;
    private readonly IPeriodService _periodService;
    private readonly IPersonalFlowService _personalFlowService;
    private readonly IReportService _reportService;
    private readonly IUserControlSession userControlSession;
    private readonly IRoleService roleService;
    private readonly ITargetEmployeeSettingService _targetEmployeeSettingService;
    private readonly IDepartmentFlowService departmentFlowService;
    private readonly IWebHostEnvironment _webHostEnvironment;

#pragma warning disable S1541 // Methods and properties should not be too complex
#pragma warning disable S107 // Methods should not have too many parameters

    public ExportReportController(
#pragma warning restore S1541 // Methods and properties should not be too complex
        IReportService reportService,
        IEpaDetailReportService epaDetailReportService,
        IEpaDetailReportService monthlyEpaDetailReportService,
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
        IWebHostEnvironment webHostEnvironment)
#pragma warning restore S107 // Methods should not have too many parameters
    {
        _reportService = reportService ?? new ReportService();
        _epaDetailReportService = epaDetailReportService ?? new EpaDetailReportService();
        _monthlyEpaDetailReportService = monthlyEpaDetailReportService ?? new MonthlyEpaDetailReportService();
        _disciplineService = disciplineService ?? new DisciplineService();
        this.userControlSession = userControlSession;
        this.roleService = roleService ?? new RoleService();
        _employeeEPAService = employeeEPAService ?? new EmployeeEPAService();
        _exportExcelService = exportExcelService ?? new ExportExcelService();
        _targetEmployeeSettingService = targetEmployeeSettingService ?? new TargetEmployeeSettingService();
        _periodService = periodService ?? new PeriodService();
        _employeeService = employeeService ?? new EmployeeService();
        _personalFlowService = personalFlowService ?? new PersonalFlowService();
        this.departmentFlowService = departmentFlowService ?? new DepartmentFlowService();
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<ActionResult> ExportDisciplineSummaryForOtherSystemReport(GetDisciplineSummaryForOtherSystemCriteria criteria)
    {
        const int contentFirstRowIndex = 3;
        if (criteria != null)
        {
            var employeeDisciplines = _disciplineService.GetDisciplineSummaryForOtherSystem(criteria);

            if (employeeDisciplines != null && employeeDisciplines.Any())
            {
                _exportExcelService.FileName = "Discipline_Summary_Report";
                _exportExcelService.SheetName = "Discipline Summary";
                _exportExcelService.Title = "Discipline Summary Report";

                _exportExcelService.InitializeWithTemplate(Path.Combine(_webHostEnvironment.ContentRootPath, DisciplineSummaryByPeriodTemplatePath));

                _exportExcelService.ContentRowStartIndex = contentFirstRowIndex;
                CreateRedStyleForDisciplineSummaryByPeriodReport();

                await _exportExcelService.ExportWithTemplate(employeeDisciplines, true, true);
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
            var employeeDisciplines = _disciplineService.GetDisciplineSummary(criteria);

            if (employeeDisciplines != null && employeeDisciplines.Any())
            {
                _exportExcelService.FileName = Lang.AttendanceSummaryReportName;
                _exportExcelService.SheetName = Lang.AttendanceSummaryReportSheet;
                _exportExcelService.Title = GetDisciplineSummaryReportTitle(criteria);

                _exportExcelService.InitializeWithTemplate(Path.Combine(_webHostEnvironment.ContentRootPath, DisciplineSummaryTemplatePath));
                _exportExcelService.ContentRowStartIndex = contentFirstRowIndex;

                CreateRedStyleForDisciplineSummaryReport();

                await _exportExcelService.ExportWithTemplate(employeeDisciplines, true, true);
            }
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportPeriodEmployeeSetting(GetPeriodEmployeeSettingCriteria criteria)
    {
        var employeeFlowSettings = _periodService.GetPeriodEmployeeSettingById(criteria);

        _exportExcelService.FileName = string.Format(Lang.EmployeeFlowSettingReportName, criteria.PeriodName);
        _exportExcelService.SheetName = Lang.EmployeeFlowSettingReportSheet;
        _exportExcelService.Title = string.Format(Lang.EmployeeFlowSettingReportTitle, criteria.PeriodName);
        _exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

        _exportExcelService.InitializeWithTemplate(Path.Combine(_webHostEnvironment.ContentRootPath, EmployeeFlowSettingTemplate));

        await _exportExcelService.ExportWithTemplate(employeeFlowSettings, true, true);

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportEmployeeInfoReport(GetEmployeeCriteria criteria)
    {
        if (criteria != null)
        {
            var roles = roleService.GetBySiteId(userControlSession.SiteId);
            IEnumerable<Employee> employees = _employeeService.GetEmployee(criteria, roles);

            if (employees.Any())
            {
                _exportExcelService.FileName = "Employee_Infomation";
                _exportExcelService.SheetName = "Employee Infomation";

                _exportExcelService.InitializeWithTemplate(Path.Combine(_webHostEnvironment.ContentRootPath, EmployeeBasicInfoTemplate));

                _exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

                await _exportExcelService.ExportWithTemplate(employees, true);
            }
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportEPAAfterUpdateFinalScore(GetEPAToUpdateFinalScoreCriteria criteria)
    {
        if (criteria != null)
        {
            var epaInfos = _employeeEPAService.GetEPAToUpdateFinalScore(criteria);
            var employeeIds = criteria.EmployeeIds.Split(',');
            epaInfos = epaInfos.Join(employeeIds, x => x.Employee.EmployeeId.ToString(), y => y, (x, y) => x)
                .ToList();

            if (epaInfos.Any())
            {
                _exportExcelService.FileName = "Update_Final_Score";
                _exportExcelService.SheetName = "Update Final Score";
                _exportExcelService.Title = criteria.PeriodName;

                _exportExcelService.InitializeWithTemplate(Path.Combine(_webHostEnvironment.ContentRootPath, UpdateFinalScoreTemplatePath));

                _exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

                await _exportExcelService.ExportWithTemplate(epaInfos, true, true);
            }
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportEPADetailReport(GetEPADetailCriteria criteria)
    {
        criteria.ActorId = userControlSession.Get<int>(Constants.CurrentEmployeeIdKey);

        var roles = roleService.GetBySiteId(userControlSession.SiteId);
        var epaDetail = _reportService.GetEPADetailReport(criteria, criteria.ActorId, roles);

        epaDetail.Period = _reportService.GetEPADetailPeriodForExportedFileName(criteria.PeriodId?.ToString());

        if (string.IsNullOrEmpty(criteria.SortColumnName) && epaDetail.EpaInfos.Any())
        {
            epaDetail.EpaInfos = epaDetail.EpaInfos.First().Period.TargetTypeId == (byte)TargetType.Probation
                ? epaDetail.EpaInfos.OrderByDescending(x => x.Employee.Code)
                : epaDetail.EpaInfos.OrderBy(x => x.Employee.GroupCode).ThenBy(x => x.Employee.Code);
        }

        _epaDetailReportService.Render(epaDetail);

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportEPASummaryReport(GetEPASummaryCriteria criteria)
    {
        criteria.ActorId = userControlSession.Get<int>(Constants.CurrentEmployeeIdKey);

        var epaSummary = _reportService.GetEPASummaryReport(criteria);
        epaSummary.Period = _reportService.GetEPADetailPeriodForExportedFileName(criteria.PeriodIds);

        var output = _reportService.ExportEPASummaryReport(epaSummary);

        var fileName = GetEPASummaryReportFilename(epaSummary);

        return File(output.ToArray(), "application/vnd.ms-excel", fileName);
    }

    public async Task<ActionResult> ExportMonthlyEPADetailReport(GetMonthlyEPADetailCriteria criteria)
    {
        criteria.EmployeeId = userControlSession.Get<int>(Constants.CurrentEmployeeIdKey);

        var roles = roleService.GetBySiteId(userControlSession.SiteId);
        var epaDetail = _reportService.GetMonthlyEPADetailReport(criteria, criteria.EmployeeId, roles);
        epaDetail.Period = _reportService.GetEPADetailPeriodForExportedFileName(criteria.PeriodId?.ToString());
        _monthlyEpaDetailReportService.Render(epaDetail);

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportMonthlyEPASummaryReport(GetMonthlyEPASummaryCriteria criteria)
    {
        var fileName = Lang.MonthlyEPASummaryReportName + ".xls";

        criteria.EmployeeId = userControlSession.Get<int>(Constants.CurrentEmployeeIdKey);

        var epaSummary = _reportService.GetMonthlyEPASummaryReport(criteria);

        var output = _reportService.ExportMonthlyEPASummaryReport(epaSummary);

        return File(output.ToArray(), "application/vnd.ms-excel", fileName);
    }

    public async Task<ActionResult> ExportPersonalFlowSetting(int flowTemplateId, string flowTemplateName)
    {
        if (flowTemplateId > 0 && !string.IsNullOrEmpty(flowTemplateName))
        {
            var flowSettings = _personalFlowService.GetByFlowTemplateId(flowTemplateId);

            _exportExcelService.FileName = "Personal_Flow_Setting";
            _exportExcelService.SheetName = "Personal Flow";
            _exportExcelService.Title = $"{flowTemplateName} - Personal Flow Setting";

            _exportExcelService.InitializeWithTemplate(Path.Combine(_webHostEnvironment.ContentRootPath, PersonalFlowSettingTemplate));

            _exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

            await _exportExcelService.ExportWithTemplate(flowSettings, true, true);
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportGroupFlowSetting(int flowTemplateId, string flowTemplateName)
    {
        if (flowTemplateId > 0 && !string.IsNullOrEmpty(flowTemplateName))
        {
            var flowSettings = departmentFlowService.GetByFlowTemplateId(flowTemplateId);

            _exportExcelService.FileName = "Department_Flow_Setting";
            _exportExcelService.SheetName = "Department Flow";
            _exportExcelService.Title = $"{flowTemplateName} - Department Flow Setting";

            _exportExcelService.InitializeWithTemplate(Path.Combine(_webHostEnvironment.ContentRootPath, GroupFlowSettingTemplate));

            _exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

            await _exportExcelService.ExportWithTemplate(flowSettings, true, true);
        }

        return new EmptyResult();
    }

    public async Task<ActionResult> ExportTargetPersonnelSettingReport(GetAllTargetEmployeeSettingCriteria criteria)
    {
        if (criteria != null)
        {
            var datas = _targetEmployeeSettingService.GetTargetEmployeeSetting(criteria);

            _exportExcelService.FileName = "Target_Personnel_Setting";
            _exportExcelService.SheetName = "Target Personnel Setting";
            _exportExcelService.Title = "Target Personnel Setting";

            _exportExcelService.InitializeWithTemplate(Path.Combine(_webHostEnvironment.ContentRootPath, TargetPersonnelSettingTemplate));
            _exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

            await _exportExcelService.ExportWithTemplate(datas, true);
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
        var redStyleIndex = _exportExcelService.CreateNewColumnStyle();
        var style = _exportExcelService.ColumnsStyles[redStyleIndex];
        var redFont = _exportExcelService.Workbook.CreateFont();
        redFont.Color = HSSFColor.Red.Index;
        style.SetFont(redFont);

        for (var i = 8; i <= lastCollumnIndex; i++)
        {
            var redStyle = CreateCellStyleIfLessThanZero(redStyleIndex, i);
            _exportExcelService.ConditionalCellStyles.Add(redStyle);
        }
    }
}