using GO.UserControl.Client.Core.Models.Role;
using GO.UserControl.Client.Core.Session;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.EmployeeEPA;
using SMS.EPA.Models.EmployeeEPA;
using SMS.EPA.Services.Implement;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Site.Controllers.API;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GO.UserControl.Client.Core.Enumerables;
    
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Enumerations;
using SMS.EPA.Core.Model;
using SMS.EPA.Core.Services;
using SMS.EPA.Criteria;
using SMS.EPA.Services;

public class EvaluationController(
    IUserControlSession userControlSession,
    IEmployeeEPAService evaluationService,
    IBaseService baseService)
    : BaseController(userControlSession)
{
    private readonly IEmployeeEPAService evaluationService = evaluationService ?? new EmployeeEPAService();
    private readonly IBaseService baseService = baseService ?? new BaseService();

    public EvaluationController() : this(null, null, null)
    {
    }

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

        var roles = userControlSession.Get<IList<Role>>(Constants.CurrentEmployeeRoleKey);
        evaluationService.RemoveSensitiveDataOnEmployeeRole(epaInfo, CurrentEmployeeId, roles);

        return Ok(epaInfo);
    }

    [HttpPut]
    public CommandResult ConvertEPA()
    {
        IEnumerable<EPASummaryImportModel> epaInfos = baseService.GetData<EPASummaryImportModel>(new GetEPASummaryImportCriteria());
        var criteria = new UpdateEPASummaryImportCriteria
        {
            EpaInfos = epaInfos
        };

        CommandResult result = baseService.ExecuteNonQuery(criteria);
        return result;
    }

    [HttpPut]
    public IActionResult ResetScore(ResetScoreCriteria criteria)
    {
        IActionResult response = null;

        var roles = userControlSession.Get<IList<Role>>(Constants.CurrentEmployeeRoleKey);
        if (roles.Select(x => x.RoleId).Contains((byte)SystemRole.SystemAdmin))
        {
            EPAModel epaInfo = evaluationService.GetEPAInfoById(criteria.EPAId);

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