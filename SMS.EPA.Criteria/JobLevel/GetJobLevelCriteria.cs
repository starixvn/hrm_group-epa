using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.JobLevel;

public class GetJobLevelCriteria : CriteriaBase
{
    public bool? IsDeleted { get; set; }

    public override string GetSettingKey() => "JobLevel_Get";

    public override bool IsValid() => true;
}