using System;
using System.Xml.Serialization;
using SMS.EPA.Core.Extensions.XML;

namespace SMS.EPA.Models.AutoService;

[Serializable]
[XmlName("Row")]
public class AutoEvaluationProcessModel
{
    public int EPAId { get; set; }
    public int PeriodId { get; set; }
    public byte TargetTypeId { get; set; }
    public int EvaluatorId { get; set; }
    public byte ProcessId { get; set; }
    public string TotalScore { get; set; }
    public string AdjustScore { get; set; }

    [XmlIgnore]
    public bool HasMonthlyEPA { get; set; }

    [XmlIgnore]
    public bool IsSemiHasMonthly => TargetTypeId == (byte)Core.Enumerations.TargetType.SemiAnnual && HasMonthlyEPA;

    [XmlIgnore]
    public bool IsChanged { get; set; }
}