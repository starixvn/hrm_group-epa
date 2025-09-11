using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.Department;

public class GetOrgChartCriteria : CriteriaBase
{
    public override bool IsValid() => true;

    public override string GetSettingKey() => "Department_GetOrgChart";
}