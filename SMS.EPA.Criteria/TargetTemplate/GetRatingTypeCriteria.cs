using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.TargetTemplate;

public class GetRatingTypeCriteria : CriteriaBase
{
    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey()
    {
        return "EpaRatingType_Get";
    }
}