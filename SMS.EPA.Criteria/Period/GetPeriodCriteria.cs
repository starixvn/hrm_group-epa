using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.Period;

public class GetPeriodCriteria : CriteriaBase
{
    public int? TargetTypeId { get; set; }

    public bool? IsActive { get; set; }

    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey()
    {
        return "EpaPeriod_Get";
    }
}