using System;

namespace SMS.EPA.Models.Dashboard;

public class JobLevelStandardProcessSetting
{
    public int PeriodId { get; set; }
    public string PeriodName { get; set; }

    public byte ProcessId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}