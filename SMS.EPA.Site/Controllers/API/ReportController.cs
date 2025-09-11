using GO.UserControl.Client.Core.Models.Role;
using SMS.EPA.Models.Employee;
using SMS.EPA.Models.Period;
using SMS.EPA.Models.Report.EPADetail;
using SMS.EPA.Models.Report.EPASummary;
using SMS.EPA.Services.Implement;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GO.UserControl.Client.Core.Enumerables;
    
using GO.UserControl.Client.Core.Services;
using GO.UserControl.Client.Core.Session;
using SMS.EPA.Core.Attribute;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Enumerations;
using SMS.EPA.Criteria.EPAHistory;
using SMS.EPA.Criteria.Report;
using SMS.EPA.Services;

public class ReportController(
    IUserControlSession userControlSession,
    IRoleService roleService,
    IReportService reportService,
    ICommonService commonService)
    : BaseController(userControlSession)
{
    private readonly IRoleService roleService = roleService ?? new RoleService();
    private readonly IReportService reportService = reportService ?? new ReportService();
    private readonly ICommonService commonService = commonService ?? new CommonService();

    [ApiAuthorize(FunctionKey = EPAFunctionKey.EPASummaryReport, Policy = Policy.Read)]

    public IActionResult GetEPASummaryReport(
        string periodIds = null,
        string teamIds = null,
        bool? isLocal = null,
        int? employeeId = null,
        string sortColumnName = null,
        string sortDirection = null)
    {
        var criteria = new GetEPASummaryCriteria
        {
            ActorId = CurrentEmployeeId,
            EmployeeId = employeeId,
            PeriodIds = periodIds,
            TeamIds = teamIds,
            IsLocal = isLocal,
            SortColumnName = sortColumnName,
            SortDirection = sortDirection
        };

        EPASummaryModel epaInfos = reportService.GetEPASummaryReport(criteria);

        return Ok(epaInfos);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.EPADetailReport, Policy = Policy.Read)]

    public IActionResult GetEPADetailReport(
        int? periodId = null,
        string teamIds = null,
        bool? isLocal = null,
        string phaseIds = null,
        int? employeeId = null,
        string sortColumnName = null,
        string sortDirection = null)
    {
        var criteria = new GetEPADetailCriteria
        {
            ActorId = CurrentEmployeeId,
            EmployeeId = employeeId,
            PeriodId = periodId,
            TeamIds = teamIds,
            IsLocal = isLocal,
            PhaseIds = phaseIds,
            SortColumnName = sortColumnName,
            SortDirection = sortDirection
        };

        IList<Role> roles = roleService.GetBySiteId(userControlSession.SiteId);
        EPADetailModel epaInfos = reportService.GetEPADetailReport(criteria, CurrentEmployeeId, roles);

        if (string.IsNullOrEmpty(sortColumnName) && epaInfos.EpaInfos.Any())
        {
            epaInfos.EpaInfos = epaInfos.EpaInfos.First().Period.TargetTypeId == (byte)TargetType.Probation
                ? epaInfos.EpaInfos.OrderByDescending(x => x.Employee.Code)
                : epaInfos.EpaInfos.OrderBy(x => x.Employee.GroupCode).ThenBy(x => x.Employee.Code);
        }

        return Ok(epaInfos);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.MonthlyEPADetailReport, Policy = Policy.Read)]

    public IActionResult GetMonthlyEPADetailReport(
        int? periodId = null,
        string teamIds = null,
        bool? isLocal = null,
        string phaseIds = null,
        string sortColumnName = null,
        string sortDirection = null)
    {
        var criteria = new GetMonthlyEPADetailCriteria
        {
            EmployeeId = CurrentEmployeeId,
            PeriodId = periodId,
            TeamIds = teamIds,
            IsLocal = isLocal,
            PhaseIds = phaseIds,
            SortColumnName = sortColumnName,
            SortDirection = sortDirection
        };

        IList<Role> roles = roleService.GetBySiteId(userControlSession.SiteId);
        EPADetailModel epaInfos = reportService.GetMonthlyEPADetailReport(criteria, CurrentEmployeeId, roles);

        return Ok(epaInfos);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.MonthlyEPASummaryReport, Policy = Policy.Read)]

    public IActionResult GetMonthlyEPASummaryReport(
        string periodIds = null,
        string teamIds = null,
        bool? isLocal = null,
        string sortColumnName = null,
        string sortDirection = null)
    {
        var criteria = new GetMonthlyEPASummaryCriteria
        {
            EmployeeId = CurrentEmployeeId,
            PeriodIds = periodIds,
            TeamIds = teamIds,
            IsLocal = isLocal,
            SortColumnName = sortColumnName,
            SortDirection = sortDirection
        };

        EPASummaryModel epaInfos = reportService.GetMonthlyEPASummaryReport(criteria);

        return Ok(epaInfos);
    }

    public IEnumerable<PeriodModel> GetAllPeriodIncludeImport()
    {
        var criteria = new GetAllPeriodIncludeImportCriteria();
        IEnumerable<PeriodModel> periods = commonService.GetAllPeriodIncludeImport(criteria);

        if (periods.Any())
        {
            periods = periods.OrderByDescending(x => x.Year).ToList();
        }

        return periods;
    }

    public IEnumerable<EmployeeViewModel> GetEmployeeByActorId(
        string query = "",
        string periodIds = null,
        string teamIds = null,
        bool? isLocal = null)
    {
        var criteria = new GetEmployeeByActorIdCriteria
        {
            Query = query,
            PeriodIds = periodIds,
            TeamIds = teamIds,
            IsLocal = isLocal,
            ActorId = CurrentEmployeeId
        };
        return reportService.GetEmployeeByActorId(criteria);
    }
}