using System.Collections.Generic;
using Fanex.Data;
using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.JobLevelStandard.JobLevelSetting;

public class EditJobLevelSettingCommand : CriteriaBase
{
    public string GroupIds { get; set; }

    [SpParam(Ignored = true)]
    public IList<Models.JobLevelStandard.JobLevelSetting.JobLevelSetting> Settings { get; set; }

    public string SettingXml { get; set; }
    public int ModifiedBy { get; set; }

    public override bool IsValid() => !string.IsNullOrWhiteSpace(GroupIds) && ModifiedBy > 0;

    public override string GetSettingKey() => "JobLevelStandard_JobLevelSetting_Update";
}