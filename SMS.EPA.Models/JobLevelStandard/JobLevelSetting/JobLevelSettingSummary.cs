using System;

namespace SMS.EPA.Models.JobLevelStandard.JobLevelSetting;

[Serializable]
public class JobLevelSettingSummary
{
    public int GroupId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }

    public int DepartmentId { get; set; }
    public string Department { get; set; }

    public int UnitId { get; set; }
    public string Unit { get; set; }

    public int CompanyId { get; set; }
    public string Company { get; set; }

    public bool HasSetting { get; set; }
}