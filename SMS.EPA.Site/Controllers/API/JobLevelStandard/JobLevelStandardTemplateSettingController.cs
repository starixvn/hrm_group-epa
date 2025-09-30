using Microsoft.AspNetCore.Mvc;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Models.JobLevelStandard.TemplateSetting;

namespace SMS.EPA.Site.Controllers.API.JobLevelStandard;

using System.Collections.Generic;
using GO.UserControl.Client.Core.Session;
using System.Linq;
using GO.UserControl.Client.Core.Enumerables;
using Core.Attribute;
using Core.Constants;
using Core.Model;
using SMS.EPA.Criteria.JobLevelStandard.TemplateSetting;

public class JobLevelStandardTemplateSettingController(IUserControlSession userControlSession, IBaseService baseService)
    : BaseController(userControlSession)
{
    [HttpGet]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardTemplateSetting, Policy = Policy.Read)]
    public IEnumerable<JobLevelStandardTemplateSetting> GetTemplateSetting([FromQuery] GetJobLevelStandardTemplateSettingCriteria criteria)
    {
        return baseService.GetData<JobLevelStandardTemplateSetting>(criteria).ToList();
    }


    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardTemplateSetting, Policy = Policy.Host)]
    public CommandResult Delete(int objectId, byte objectTypeId)
    {
        var criteria = new DeleteJobLevelStandardTemplateSettingCriteria
        {
            ObjectId = objectId,
            ObjectTypeId = objectTypeId,
            ModifiedBy = CurrentEmployeeId
        };
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardTemplateSetting, Policy = Policy.Host)]
    public CommandResult Assign([FromBody] AssignJobLevelStandardTemplateSettingCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    public byte GetUserPermission()
    {
        return GetPermissionOnFunction(EPAFunctionKey.JobLevelStandardTemplateSetting);
    }
}