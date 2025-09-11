using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Models.JobLevelStandard.Template;

namespace SMS.EPA.Site.Controllers.API.JobLevelStandard;

using System.Collections.Generic;
using GO.UserControl.Client.Core.Session;
using System.Web.Http;
using GO.UserControl.Client.Core.Enumerables;
using SMS.EPA.Core.Attribute;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Model;
using SMS.EPA.Core.Services;
using SMS.EPA.Criteria.JobLevelStandard.Template;

public class JobLevelStandardTemplateController(IUserControlSession userControlSession, IBaseService baseService)
    : BaseController(userControlSession)
{
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardTemplate, Policy = Policy.Read)]
    public IEnumerable<JobLevelStandardTemplate> GetTemplate([FromUri] GetJobLevelStandardTemplateCriteria criteria)
    {
        return baseService.GetData<JobLevelStandardTemplate>(criteria);
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardTemplate, Policy = Policy.Host)]
    public CommandResult Create(CreateJobLevelStandardTemplateCriteria criteria)
    {
        criteria.CreatedBy = CurrentEmployeeId;
        return baseService.ExecuteNonQuery(criteria);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardTemplate, Policy = Policy.Host)]
    public CommandResult UpdateStatus(UpdateStatusJobLevelStandardTemplateCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        CommandResult commandResult = baseService.ExecuteNonQuery(criteria);

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
        CommandResult commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

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
    public CommandResult Update(UpdateJobLevelStandardTemplateCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        return baseService.ExecuteNonQuery(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardTemplate, Policy = Policy.Read)]

    public IEnumerable<JobLevelStandardTemplateKPI> GetKPI([FromUri] GetKPIListCriteria criteria)
    {
        return baseService.GetData<JobLevelStandardTemplateKPI>(criteria);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardTemplate, Policy = Policy.Host)]
    public CommandResult UpdateTemplateKPI(UpdateJobLevelStandardTemplateKPICriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        CommandResult commandResult = baseService.ExecuteNonQuery(criteria);
        return commandResult;
    }

    public byte GetUserPermission()
    {
        return GetPermissionOnFunction(EPAFunctionKey.JobLevelStandardTemplate);
    }
}