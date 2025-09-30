using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.Period;

public class GetEvaluationGlobalSettingByTargetTypeIdCriteria : CriteriaBase
{
    public byte TargetTypeId { get; set; }

    public override bool IsValid() => TargetTypeId > 0;

    public override string GetSettingKey() => "EpaPeriod_GetEvaluationGlobalSettingByTargetTypeId";
}