using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Models.JobLevelStandard.KPI;

namespace SMS.EPA.Site.Controllers.API.JobLevelStandard;

using System.Collections.Generic;
using GO.UserControl.Client.Core.Session;
using System.Web.Http;
using GO.UserControl.Client.Core.Enumerables;
using SMS.EPA.Core.Attribute;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Model;
using SMS.EPA.Core.Services;
using SMS.EPA.Criteria;
using SMS.EPA.Criteria.JobLevelStandard.KPI;

public class JobLevelStandardKPIController(IUserControlSession userControlSession, IBaseService baseService)
    : BaseController(userControlSession)
{
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardKPI, Policy = Policy.Read)]

    public IEnumerable<JobLevelStandardKPI> GetKPI([FromUri] GetJobLevelStandardKPICriteria criteria)
    {
        return baseService.GetData<JobLevelStandardKPI>(criteria);
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardKPI, Policy = Policy.Host)]
    public CommandResult Create(CreateJobLevelStandardKPICriteria criteria)
    {
        criteria.CreatedBy = CurrentEmployeeId;
        return baseService.ExecuteNonQuery(criteria);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardKPI, Policy = Policy.Host)]
    public CommandResult Update(UpdateJobLevelStandardKPICriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        return baseService.ExecuteNonQuery(criteria);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardKPI, Policy = Policy.Host)]
    public CommandResult UpdateStatus(UpdateStatusJobLevelStandardKPICriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        CommandResult commandResult = baseService.ExecuteNonQuery(criteria);
        return commandResult;
    }


    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelStandardKPI, Policy = Policy.Host)]
    public CommandResult Delete(int kpiId)
    {
        var criteria = new DeleteJobLevelStandardKPICriteria
        {
            KPIId = kpiId,
            ModifiedBy = CurrentEmployeeId
        };
        CommandResult commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    public byte GetUserPermission()
    {
        return GetPermissionOnFunction(EPAFunctionKey.JobLevelStandardKPI);
    }
}