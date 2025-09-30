using Microsoft.AspNetCore.Mvc;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Models.JobLevelStandard.Template;

namespace SMS.EPA.Site.Controllers.API.JobLevelStandard;

using System.Collections.Generic;
using GO.UserControl.Client.Core.Session;
using GO.UserControl.Client.Core.Enumerables;
using Core.Attribute;
using Core.Constants;
using Core.Model;
using SMS.EPA.Criteria.JobLevelStandard.Template;

public class JobLevelStandardTemplateController(IUserControlSession userControlSession, IBaseService baseService)
    : BaseController(userControlSession)
{
    [HttpGet]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardTemplate, Policy = Policy.Read)]
    public IEnumerable<JobLevelStandardTemplate> GetTemplate([FromQuery] GetJobLevelStandardTemplateCriteria criteria)
    {
        return baseService.GetData<JobLevelStandardTemplate>(criteria);
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardTemplate, Policy = Policy.Host)]
    public CommandResult Create([FromBody] CreateJobLevelStandardTemplateCriteria criteria)
    {
        criteria.CreatedBy = CurrentEmployeeId;
        return baseService.ExecuteNonQuery(criteria);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardTemplate, Policy = Policy.Host)]
    public CommandResult UpdateStatus([FromBody] UpdateStatusJobLevelStandardTemplateCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }


    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardTemplate, Policy = Policy.Host)]
    public CommandResult Delete(int templateId)
    {
        var criteria = new DeleteJobLevelStandardTemplateCriteria
        {
            TemplateId = templateId,
            ModifiedBy = CurrentEmployeeId
        };
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    [HttpGet]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardTemplate, Policy = Policy.Read)]
    public JobLevelStandardTemplate GetById(int templateId)
    {
        var criteria = new GetJobLevelStandardTemplateByIdCriteria
        {
            TemplateId = templateId
        };
        return baseService.GetDataEntity<JobLevelStandardTemplate>(criteria);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardTemplate, Policy = Policy.Host)]
    public CommandResult Update([FromBody] UpdateJobLevelStandardTemplateCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        return baseService.ExecuteNonQuery(criteria);
    }

    [HttpGet]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardTemplate, Policy = Policy.Read)]
    public IEnumerable<JobLevelStandardTemplateKPI> GetKPI([FromQuery] GetKPIListCriteria criteria)
    {
        return baseService.GetData<JobLevelStandardTemplateKPI>(criteria);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardTemplate, Policy = Policy.Host)]
    public CommandResult UpdateTemplateKPI([FromBody] UpdateJobLevelStandardTemplateKPICriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        var commandResult = baseService.ExecuteNonQuery(criteria);
        return commandResult;
    }

    public byte GetUserPermission()
    {
        return GetPermissionOnFunction(EPAFunctionKey.JobLevelStandardTemplate);
    }
}