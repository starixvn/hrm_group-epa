namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using System.Linq;
using Core.Attribute;
using Core.Constants;
using Core.Enumerations;
using Criteria.EPAHistory;
using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Models.Role;
using GO.UserControl.Client.Core.Session;
using Services.Interfaces;
using SMS.EPA.Models.EPAHistory;

public class EPAHistoryController(
    IUserControlSession userControlSession,
    IEPAHistoryService epaHistoryService) : BaseController(userControlSession)
{
    [ApiAuthorize(FunctionKey = EPAFunctionKey.EPAHistory, Policy = Policy.Read)]
    public IList<EPAHistoryModel> GetEPAHistory(GetEPAHistoryCriteria criteria)
    {
        criteria.EvaluatorId = CurrentEmployeeId;
        var epas = epaHistoryService.GetEPAHistory(criteria);
        return epas;
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.MonthlyEPAHistory, Policy = Policy.Read)]
    public IList<EPAHistoryModel> GetMonthlyEPAHistory(int? periodId = null, int? employeeId = null)
    {
        var criteria = new GetMonthlyEPAHistoryCriteria
        {
            PeriodId = periodId,
            EvaluatorId = CurrentEmployeeId,
            EmployeeId = employeeId
        };

        var epas = epaHistoryService.GetMonthlyEPAHistory(criteria);
        return epas;
    }

    public bool HasOnlyEmployeeRole()
    {
        var roles = userControlSession.Get<IList<Role>>(Constants.CurrentEmployeeRoleKey);
        return roles != null && roles.All(x => x.RoleId == (byte)SystemRole.Employee);
    }
}