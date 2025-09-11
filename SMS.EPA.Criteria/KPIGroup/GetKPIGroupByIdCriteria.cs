using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.KPIGroup;

public class GetKPIGroupByIdCriteria : CriteriaBase
{
    public int KPIGroupId { get; set; }

    public override bool IsValid() => KPIGroupId > 0;

    public override string GetSettingKey() => "KPIGroup_GetById";
}