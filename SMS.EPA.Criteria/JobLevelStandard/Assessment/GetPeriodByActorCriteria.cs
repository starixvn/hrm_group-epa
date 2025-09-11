using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.JobLevelStandard.Assessment;

public class GetPeriodByActorCriteria : CriteriaBase
{
    public int ActorId { get; set; }

    public override string GetSettingKey() => "JobLevelStandard_Assessment_GetPeriodByActorId";

    public override bool IsValid() => ActorId > 0;
}