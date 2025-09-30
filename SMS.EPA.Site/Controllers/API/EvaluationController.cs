using GO.UserControl.Client.Core.Models.Role;
using GO.UserControl.Client.Core.Session;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.EmployeeEPA;
using SMS.EPA.Models.EmployeeEPA;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Site.Controllers.API;

using System;
using System.Collections.Generic;
using System.Linq;
using Core.Constants;
using Core.Enumerations;
using Core.Model;
using GO.UserControl.Client.Core.Enumerables;
using Microsoft.AspNetCore.Mvc;

public class EvaluationController(
    IUserControlSession session,
    IEmployeeEPAService evaluationService,
    IBaseService baseService) : BaseController(session)
{
    public IActionResult GetEPAInfoById(int epaId)
    {
        var epaInfo = new EPAModel();
        int permission = CheckEmployeePermission(epaId);

        if (permission == (byte)Policy.None)
        {
            return StatusCode(403, epaInfo);
        }

        epaInfo = evaluationService.GetEPAInfoById(epaId);

        if (epaInfo == null)
        {
            return NotFound();
        }

        var roles = session.Get<IList<Role>>(Constants.CurrentEmployeeRoleKey);
        evaluationService.RemoveSensitiveDataOnEmployeeRole(epaInfo, CurrentEmployeeId, roles);

        return Ok(epaInfo);
    }

    [HttpPut]
    public CommandResult ConvertEPA()
    {
        var epaInfos = baseService.GetData<EPASummaryImportModel>(new GetEPASummaryImportCriteria());
        var criteria = new UpdateEPASummaryImportCriteria
        {
            EpaInfos = epaInfos
        };

        var result = baseService.ExecuteNonQuery(criteria);
        return result;
    }

    [HttpPut]
    public IActionResult ResetScore(ResetScoreCriteria criteria)
    {
        IActionResult response = null;

        var roles = session.Get<IList<Role>>(Constants.CurrentEmployeeRoleKey);
        if (roles.Select(x => x.RoleId).Contains((byte)SystemRole.SystemAdmin))
        {
            var epaInfo = evaluationService.GetEPAInfoById(criteria.EPAId);

            criteria.ModifiedBy = CurrentEmployeeId;

            CommandResult commandResult;
            if (epaInfo.IsSemiHasMonthly)
            {
                if (epaInfo.Process.Reviewers.Any()
                    && epaInfo.Process.Reviewers[0].StartDate.Date <= DateTimeOffset.Now.Date)
                {
                    criteria.EPAInfo = epaInfo;

                    commandResult = baseService.ExecuteNonQuery(criteria);
                    response = Ok(commandResult);
                }
            }
            else
            {
                criteria.EPAInfo = epaInfo;

                commandResult = baseService.ExecuteNonQuery(criteria);
                response = Ok(commandResult);
            }
        }
        else
        {
            response = StatusCode(403);
        }

        return response;
    }

    private byte CheckEmployeePermission(int epaId)
    {
        var criteria = new GetEmployeePermissionCriteria
        {
            EPAId = epaId,
            ActorUserId = CurrentEmployeeId
        };

        return baseService.GetDataEntity<byte>(criteria);
    }
}