using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.JobLevelStandard.Period;

public class GetJobLevelStandardPeriodByIdCriteria : CriteriaBase
{
    public int PeriodId { get; set; }

    public override string GetSettingKey() => "JobLevelStandard_Period_GetById";

    public override bool IsValid() => PeriodId > 0;
}