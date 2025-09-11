using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.JobLevelStandard.KPI;

public class CreateJobLevelStandardKPICriteria : CriteriaBase
{
    public int KPIId { get; set; }

    public string Name { get; set; }

    public int CreatedBy { get; set; }

    public override string GetSettingKey() => "JobLevelStandard_KPI_Insert";

    public override bool IsValid() => CreatedBy > 0 && !string.IsNullOrWhiteSpace(Name);
}