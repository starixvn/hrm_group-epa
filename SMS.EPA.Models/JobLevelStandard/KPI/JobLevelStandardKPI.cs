using System;

namespace SMS.EPA.Models.JobLevelStandard.KPI;

[Serializable]
public class JobLevelStandardKPI
{
    public int KPIId { get; set; }
    public string Name { get; set; }
    public int? DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}