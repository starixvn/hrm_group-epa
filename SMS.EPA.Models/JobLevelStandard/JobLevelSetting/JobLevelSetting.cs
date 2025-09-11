using System;
using System.Xml.Serialization;
using SMS.EPA.Core.Extensions.XML;

namespace SMS.EPA.Models.JobLevelStandard.JobLevelSetting;

[Serializable]
[XmlName("Row")]
public class JobLevelSetting
{
    public int SectionId { get; set; }
    public int JobLevelId { get; set; }

    [XmlIgnore]
    public string Name { get; set; }

    public decimal? MinWeight { get; set; }
    public decimal? MaxWeight { get; set; }
}