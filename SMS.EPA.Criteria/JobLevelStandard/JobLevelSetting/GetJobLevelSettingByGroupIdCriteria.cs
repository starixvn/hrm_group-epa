using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.JobLevelStandard.JobLevelSetting;

public class GetJobLevelSettingByGroupIdCriteria : CriteriaBase
{
    public int GroupId { get; set; }

    public override string GetSettingKey() => "JobLevelStandard_JobLevelSetting_GetByGroupId";

    public override bool IsValid() => true;
}