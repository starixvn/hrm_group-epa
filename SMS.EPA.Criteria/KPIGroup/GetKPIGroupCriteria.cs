using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.KPIGroup;

public class GetKPIGroupCriteria : CriteriaBase
{
    public bool? IsActive { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "KPIGroup_Get";
}