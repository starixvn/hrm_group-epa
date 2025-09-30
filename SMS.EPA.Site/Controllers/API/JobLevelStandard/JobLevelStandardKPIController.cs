using System.Collections.Generic;
using GO.UserControl.Client.Core.Session;
using GO.UserControl.Client.Core.Enumerables;
using SMS.EPA.Criteria.JobLevelStandard.KPI;
using Microsoft.AspNetCore.Mvc;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Models.JobLevelStandard.KPI;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Attribute;
using SMS.EPA.Core.Model;

namespace SMS.EPA.Site.Controllers.API.JobLevelStandard;

public class JobLevelStandardKPIController(IUserControlSession userControlSession, IBaseService baseService)
    : BaseController(userControlSession)
{
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardKPI, Policy = Policy.Read)]
    public IEnumerable<JobLevelStandardKPI> GetKPI([FromQuery] GetJobLevelStandardKPICriteria criteria)
    {
        return baseService.GetData<JobLevelStandardKPI>(criteria);
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardKPI, Policy = Policy.Host)]
    public CommandResult Create([FromBody] CreateJobLevelStandardKPICriteria criteria)
    {
        criteria.CreatedBy = CurrentEmployeeId;
        return baseService.ExecuteNonQuery(criteria);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardKPI, Policy = Policy.Host)]
    public CommandResult Update([FromBody] UpdateJobLevelStandardKPICriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        return baseService.ExecuteNonQuery(criteria);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardKPI, Policy = Policy.Host)]
    public CommandResult UpdateStatus([FromBody] UpdateStatusJobLevelStandardKPICriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        var commandResult = baseService.ExecuteNonQuery(criteria);
        return commandResult;
    }

    [HttpDelete]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardKPI, Policy = Policy.Host)]
    public CommandResult Delete(int kpiId)
    {
        var criteria = new DeleteJobLevelStandardKPICriteria
        {
            KPIId = kpiId,
            ModifiedBy = CurrentEmployeeId
        };
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    public byte GetUserPermission()
    {
        return GetPermissionOnFunction(EPAFunctionKey.JobLevelStandardKPI);
    }
}