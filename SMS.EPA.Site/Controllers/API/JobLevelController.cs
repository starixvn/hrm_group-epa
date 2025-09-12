using SMS.EPA.Core.Services.Interfaces;

namespace SMS.EPA.Site.Controllers.API;

using System.Web.Http;
using GO.UserControl.Client.Core.Session;
using GO.UserControl.Client.Core.Enumerables;
using Core.Attribute;
using Core.Constants;
using Core.Model;
using Criteria.JobLevel;

public class JobLevelController(IUserControlSession userControlSession, IBaseService baseService)
    : BaseController(userControlSession)
{
    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevel, Policy = Policy.Host)]
    public CommandResult Create(CreateJobLevelCriteria criteria)
    {
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevel, Policy = Policy.Host)]
    public CommandResult Edit(EditJobLevelCriteria criteria)
    {
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }


    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevel, Policy = Policy.Host)]
    public CommandResult Delete(int jobLevelId)
    {
        var criteria = new DeleteJobLevelCriteria
        {
            JobLevelId = jobLevelId
        };
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    public byte GetUserPermission()
    {
        return GetPermissionOnFunction(EPAFunctionKey.JobLevel);
    }
}