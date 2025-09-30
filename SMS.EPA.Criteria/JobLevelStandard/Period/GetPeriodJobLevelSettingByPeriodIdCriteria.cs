using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.JobLevelStandard.Period;

public class GetPeriodJobLevelSettingByPeriodIdCriteria : CriteriaBase
{
    public int PeriodId { get; set; }

    public override string GetSettingKey() => "JobLevelStandard_PeriodJobLevelSetting_GetByPeriodId";

    public override bool IsValid() => PeriodId > 0;
}