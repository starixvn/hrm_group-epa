using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.JobLevelStandard.Period;

public class GetJobLevelStandardPeriodCriteria : CriteriaBase
{
    public bool? IsActive { get; set; }

    public override string GetSettingKey() => "JobLevelStandard_Period_Get";

    public override bool IsValid() => true;
}