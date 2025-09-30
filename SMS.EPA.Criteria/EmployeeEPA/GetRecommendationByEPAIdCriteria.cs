using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.EmployeeEPA;

public class GetRecommendationByEPAIdCriteria : CriteriaBase
{
    public int EPAId { get; set; }

    public override string GetSettingKey() => "EPA_GetRecommendationById";

    public override bool IsValid() => EPAId > 0;
}