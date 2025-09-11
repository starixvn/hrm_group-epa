using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.KPI;

public class DeleteKPICriteria : CriteriaBase
{
    public int KPIId { get; set; }

    public int ModifiedBy { get; set; }

    public override bool IsValid() => KPIId > 0 && ModifiedBy > 0;

    public override string GetSettingKey() => "KPI_Delete";
}