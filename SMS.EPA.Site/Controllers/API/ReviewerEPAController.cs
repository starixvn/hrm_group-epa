using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.EmployeeEPA;
using SMS.EPA.Models.EmployeeEPA;
using SMS.EPA.Models.Enitites;

namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using GO.UserControl.Client.Core.Session;
using Microsoft.AspNetCore.Mvc;
using GO.UserControl.Client.Core.Enumerables;
using SMS.EPA.Core.Attribute;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Enumerations;
using SMS.EPA.Core.Model;
using SMS.EPA.Core.Services;
using SMS.EPA.Criteria;
using SMS.EPA.Services.Implement;
using SMS.EPA.Services.Interfaces;

public class ReviewerEPAController(
    IUserControlSession userControlSession,
    IReviewerEPAService reviewerEPAService,
    IBaseService baseService)
    : BaseController(userControlSession)
{
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Reviewer, Policy = Policy.Read)]

    public IEnumerable<EPAPeriodModel> GetPeriodByReviewer()
    {
        var criteria = new GetPeriodByActorIdCriteria
        {
            ActorId = CurrentEmployeeId,
            ProcessId = (byte)ProcessType.Reviewer
        };

        return baseService.GetData<EPAPeriodModel>(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.Reviewer, Policy = Policy.Read)]

    public IEnumerable<Group> GetGroupByReviewer(int? periodId = null)
    {
        var criteria = new GetGroupByActorIdCriteria
        {
            ActorUserId = CurrentEmployeeId,
            ProcessId = (byte)ProcessType.Reviewer,
            PeriodId = periodId
        };

        return baseService.GetData<Group>(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.Reviewer, Policy = Policy.Read)]

    public IEnumerable<EPAEmployeeInfo> GetMemberByReviewer(
        int? periodId = null,
        int? groupId = null,
        bool? isLocal = null,
        bool? isMonthly = null)
    {
        var criteria = new GetMemberByActorIdCriteria
        {
            ActorUserId = CurrentEmployeeId,
            ProcessId = (byte)ProcessType.Reviewer,
            PeriodId = periodId,
            GroupId = groupId,
            IsLocal = isLocal,
            IsMonthly = isMonthly
        };

        return baseService.GetData<EPAEmployeeInfo>(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.Reviewer, Policy = Policy.Read)]

    public IEnumerable<EPAModel> GetEPAByReviewer(
        int? periodId = null,
        int? groupId = null,
        bool? isLocal = null,
        bool isMonthly = false,
        string employeeIds = null,
        bool? statusProcess = null)
    {
        var criteria = new GetEPAByReviewerCriteria
        {
            ReviewerId = CurrentEmployeeId,
            PeriodId = periodId,
            GroupId = groupId,
            IsLocal = isLocal,
            IsMonthly = isMonthly,
            EmployeeIds = employeeIds,
            StatusProcess = statusProcess
        };

        return reviewerEPAService.GetEPAByReviewer(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.Reviewer, Policy = Policy.Read)]

    public IActionResult GetEPAInfoByReviewer(int epaId, int? reviewerId = null)
    {
        var epaInfo = new EPAModel();
        var processId = reviewerId == null ? (byte?)ProcessType.Reviewer : null;
        var sessionServiceEmployeeId = reviewerId ?? CurrentEmployeeId;
        byte permission = CheckEmployeePermission(epaId, sessionServiceEmployeeId, processId);

        if (permission == (byte)Policy.None)
        {
            return StatusCode(403, epaInfo);
        }

        var criteria = new GetEPAInfoByReviewerCriteria
        {
            EPAId = epaId,
            ReviewerId = CurrentEmployeeId
        };

        epaInfo = reviewerEPAService.GetEPAInfoByReviewer(criteria);

        return Ok(epaInfo);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Reviewer, Policy = Policy.Host)]
    public CommandResult AdjustScoreByReviewer(AdjustScoreByReviewerOrManagementCriteria criteria)
    {
        var commandResult = new CommandResult();
        if (criteria != null)
        {
            criteria.ProcessId = (byte)ProcessType.Reviewer;
            criteria.ModifiedBy = CurrentEmployeeId;

            commandResult = baseService.ExecuteNonQuery(criteria);
        }

        return commandResult;
    }

    public byte GetUserPermissionOnReviewer()
    {
        return GetPermissionOnFunction(EPAFunctionKey.Reviewer);
    }

    private byte CheckEmployeePermission(
        int epaId,
        int actorUserId,
        byte? processId = null)
    {
        var criteria = new GetEmployeePermissionCriteria
        {
            EPAId = epaId,
            ActorUserId = actorUserId,
            ProcessId = processId
        };

        return baseService.GetDataEntity<byte>(criteria);
    }
}