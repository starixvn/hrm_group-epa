using System;
using System.Collections.Generic;

namespace SMS.EPA.Models.JobLevelStandard.Period;

[Serializable]
public class JobLevelStandardPeriod
{
    public int PeriodId { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }

    public IList<JobLevelStandardPeriodSetting> Settings { get; set; } = new List<JobLevelStandardPeriodSetting>();
}