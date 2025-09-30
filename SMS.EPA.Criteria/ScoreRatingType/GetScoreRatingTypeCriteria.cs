using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.ScoreRatingType;

public class GetScoreRatingTypeCriteria : CriteriaBase
{
    public int? ScoreRatingTypeId { get; set; }

    public bool IsActive { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "EpaScoreRatingType_Get";
}