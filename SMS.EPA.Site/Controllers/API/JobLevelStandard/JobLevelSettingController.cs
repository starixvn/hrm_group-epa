using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Session;
using SMS.EPA.Core.Attribute;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Extensions.XML;
using SMS.EPA.Core.Model;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.JobLevelStandard.JobLevelSetting;
using SMS.EPA.Models.JobLevelStandard.JobLevelSetting;

namespace SMS.EPA.Site.Controllers.API.JobLevelStandard;

public class JobLevelSettingController(IUserControlSession userControlSession, IBaseService baseService)
    : BaseController(userControlSession)
{

    public JobLevelSettingController() : this(null, null)
    {
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelSetting, Policy = Policy.Read)]
    public IList<JobLevelSettingSummary> GetSettings(GetJobLevelSettingSummaryCriteria criteria)
    {
        var settings = baseService.GetData<JobLevelSettingSummary>(criteria);
        return settings.ToList();
    }

    [HttpGet]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelSetting, Policy = Policy.Read)]
    public IList<JobLevelSetting> GetBySectionId(int sectionId)
    {
        var criteria = new GetJobLevelSettingBySectionIdCriteria { SectionId = sectionId };
        var settings = baseService.GetData<JobLevelSetting>(criteria);
        return settings.ToList();
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelSetting, Policy = Policy.Host)]
    public CommandResult Edit(EditJobLevelSettingCommand command)
    {
        var commandResult = new CommandResult();
        if (command != null)
        {
            command.SettingXml = command.Settings.ToXML().ToString();
            command.ModifiedBy = CurrentEmployeeId;
            commandResult = baseService.ExecuteNonQuery(command);
        }

        return commandResult;
    }

    public byte GetPermission()
    {
        return GetPermissionOnFunction(EPAFunctionKey.JobLevelSetting);
    }
}