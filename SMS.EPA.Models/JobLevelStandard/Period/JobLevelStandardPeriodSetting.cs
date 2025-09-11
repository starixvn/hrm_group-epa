using System;
using System.Xml.Serialization;
using SMS.EPA.Core.Extensions.XML;

namespace SMS.EPA.Models.JobLevelStandard.Period;

[Serializable]
[XmlName("Row")]
public class JobLevelStandardPeriodSetting
{
    [XmlIgnore]
    public int PeriodId { get; set; }

    public byte ProcessId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}