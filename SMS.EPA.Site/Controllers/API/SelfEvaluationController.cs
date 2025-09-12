using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.EmployeeEPA;
using SMS.EPA.Models.EmployeeEPA;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using GO.UserControl.Client.Core.Session;
using Microsoft.AspNetCore.Mvc;
using GO.UserControl.Client.Core.Enumerables;
using Core.Attribute;
using Core.Constants;
using Core.Enumerations;
using Core.Model;

public class SelfEvaluationController(
    IUserControlSession userControlSession,
    ISelfEvaluationService selfEvaluationService,
    IEmployeeEPAService employeeEPAService,
    IBaseService baseService)
    : BaseController(userControlSession)
{
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Self, Policy = Policy.Read)]

    public IEnumerable<EPAModel> GetEPAByEmployee()
    {
        var criteria = new GetEPAByEmployeeCriteria
        {
            EmployeeId = CurrentEmployeeId
        };

        return selfEvaluationService.GetEPAByEmployee(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.Self, Policy = Policy.Read)]

    public IActionResult GetEPAInfoByEmployee(int epaId)
    {
        var epaInfo = new EPAModel();
        var permission = CheckEmployeePermission(epaId);

        if (permission == (byte)Policy.None)
        {
            return StatusCode(403, epaInfo);
        }

        var criteria = new GetEPAInfoByEmployeeCriteria
        {
            EPAId = epaId,
            EmployeeId = CurrentEmployeeId
        };

        epaInfo = selfEvaluationService.GetEPAInfoByEmployee(criteria);

        if (epaInfo.IsProbation)
        {
            epaInfo.ExtendedQuestion = employeeEPAService.GetExtendedQuestionByEPAId(epaInfo.EPAId);
        }

        return Ok(epaInfo);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Self, Policy = Policy.Host)]
    public CommandResult UpdateEPAScoreByEmployee(UpdateEPAScoreCriteria criteria)
    {
        var commandResult = new CommandResult();
        if (criteria != null)
        {
            criteria.ProcessId = (byte)ProcessType.Self;
            criteria.EvaluatorId = CurrentEmployeeId;
            criteria.ModifiedBy = CurrentEmployeeId;

            commandResult = baseService.ExecuteNonQuery(criteria);
        }

        return commandResult;
    }

    public byte GetUserPermissionOnEmployee()
    {
        return GetPermissionOnFunction(EPAFunctionKey.Self);
    }

    private byte CheckEmployeePermission(int epaId)
    {
        var criteria = new GetEmployeePermissionCriteria
        {
            EPAId = epaId,
            ActorUserId = CurrentEmployeeId,
            ProcessId = (byte)ProcessType.Self
        };

        return baseService.GetDataEntity<byte>(criteria);
    }
}