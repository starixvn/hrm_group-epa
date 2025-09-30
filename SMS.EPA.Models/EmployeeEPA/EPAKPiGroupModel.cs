using System.Collections.Generic;

namespace SMS.EPA.Models.EmployeeEPA;

public class EPAKpiGroupModel
{
    public int EPAId { get; set; }

    public int KPIGroupId { get; set; }

    public string Name { get; set; }

    public double? Weight { get; set; }

    public bool IsSpreadKPI { get; set; }

    public int DisplayOrder { get; set; }

    public int NumberKPI { get; set; }

    public bool HasKPIOptional { get; set; }

    public int NumberKPIOptional { get; set; }

    public IList<EPAKpiModel> KPIs { get; set; } = new List<EPAKpiModel>();
}