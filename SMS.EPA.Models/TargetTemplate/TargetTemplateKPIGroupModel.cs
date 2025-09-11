using System.Collections.Generic;
using SMS.EPA.Models.KPI;
using SMS.EPA.Models.KPIGroup;

namespace SMS.EPA.Models.TargetTemplate;

public class TargetTemplateKPIGroupModel : KPIGroupModel
{
    public bool IsSpreadKPIOriginal => IsSpreadKPI;

    public IEnumerable<KPIModel> KPIs { get; set; }
}