using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.KPI;

public class GetKPICriteria : CriteriaBase
{
    public int? KPIGroupId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsRequiredEvaluate { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "KPI_Get";
}