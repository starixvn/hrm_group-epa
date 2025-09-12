using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.EmployeeEPA;
using SMS.EPA.Models.EmployeeEPA;
using SMS.EPA.Models.Enitites;

namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using GO.UserControl.Client.Core.Session;
using Microsoft.AspNetCore.Mvc;
using GO.UserControl.Client.Core.Enumerables;
using Core.Attribute;
using Core.Constants;
using Core.Enumerations;
using Core.Model;
using Services.Interfaces;

public class ManagementEPAController(
    IUserControlSession userControlSession,
    IManagementEPAService managementEPAService,
    IBaseService baseService)
    : BaseController(userControlSession)
{
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Management, Policy = Policy.Read)]

    public IEnumerable<EPAPeriodModel> GetPeriodByManagement()
    {
        var criteria = new GetPeriodByActorIdCriteria
        {
            ActorId = CurrentEmployeeId,
            ProcessId = (byte)ProcessType.Management
        };

        return baseService.GetData<EPAPeriodModel>(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.Management, Policy = Policy.Read)]

    public IEnumerable<Group> GetGroupByManagement(int? periodId = null)
    {
        var criteria = new GetGroupByActorIdCriteria
        {
            ActorUserId = CurrentEmployeeId,
            ProcessId = (byte)ProcessType.Management,
            PeriodId = periodId
        };

        return baseService.GetData<Group>(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.Management, Policy = Policy.Read)]

    public IEnumerable<EPAEmployeeInfo> GetMemberByManagement(
        int? periodId = null,
        int? groupId = null,
        bool? isLocal = null)
    {
        var criteria = new GetMemberByActorIdCriteria
        {
            ActorUserId = CurrentEmployeeId,
            ProcessId = (byte)ProcessType.Management,
            PeriodId = periodId,
            GroupId = groupId,
            IsLocal = isLocal,
            IsMonthly = false
        };

        return baseService.GetData<EPAEmployeeInfo>(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.Management, Policy = Policy.Read)]

    public IEnumerable<EPAModel> GetEPAByManagement(
        int? periodId = null,
        int? groupId = null,
        bool? isLocal = null,
        string employeeIds = null,
        bool? statusProcess = null)
    {
        var criteria = new GetEPAByManagementCriteria
        {
            ManagementId = CurrentEmployeeId,
            PeriodId = periodId,
            GroupId = groupId,
            IsLocal = isLocal,
            EmployeeIds = employeeIds,
            StatusProcess = statusProcess
        };

        return managementEPAService.GetEPAByManagement(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.Management, Policy = Policy.Read)]

    public IActionResult GetEPAInfoByManagement(int epaId, int? managementId = null)
    {
        var epaInfo = new EPAModel();

        var processId = managementId == null ? (byte?)ProcessType.Management : null;
        var sessionServiceEmployeeId = managementId ?? CurrentEmployeeId;

        var permission = CheckEmployeePermission(epaId, sessionServiceEmployeeId, processId);

        if (permission == (int)Policy.None)
        {
            return StatusCode(403, epaInfo);
        }

        var criteria = new GetEPAInfoByManagementCriteria
        {
            EPAId = epaId,
            ManagementId = CurrentEmployeeId
        };

        epaInfo = managementEPAService.GetEPAInfoByManagement(criteria);

        return Ok(epaInfo);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Management, Policy = Policy.Host)]
    public CommandResult AdjustScoreByManagement(AdjustScoreByReviewerOrManagementCriteria criteria)
    {
        var commandResult = new CommandResult();

        if (criteria != null)
        {
            criteria.ProcessId = (byte)ProcessType.Management;
            criteria.ModifiedBy = CurrentEmployeeId;

            commandResult = baseService.ExecuteNonQuery(criteria);
        }

        return commandResult;
    }

    public byte GetUserPermissionOnManagement()
    {
        return GetPermissionOnFunction(EPAFunctionKey.Management);
    }

    private int CheckEmployeePermission(
        int epaId,
        int actorUserId,
        int? processId = null)
    {
        var criteria = new GetEmployeePermissionCriteria
        {
            EPAId = epaId,
            ActorUserId = actorUserId,
            ProcessId = processId
        };

        return baseService.GetDataEntity<int>(criteria);
    }
}