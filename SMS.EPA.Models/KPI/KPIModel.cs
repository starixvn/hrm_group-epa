using System;
using System.Collections.Generic;
using SMS.EPA.Models.KPIGroup;
using SMS.EPA.Models.TargetType;

namespace SMS.EPA.Models.KPI;

public class KPIModel
{
    public int KPIId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool IsActive { get; set; }

    public bool HasComment { get; set; }

    public bool IsRequiredComment { get; set; }

    public bool IsRequiredEvaluate { get; set; }

    public bool? Selected { get; set; }

    public double? Weight { get; set; }

    public IList<KPIGroupModel> KPIGroups { get; set; } = new List<KPIGroupModel>();

    public TargetTypeModel TargetType { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }
}