using System.Collections.Generic;

namespace SMS.EPA.Models.EmployeeEPA.RatingByKPI;

public class TemporaryKPIGroupModel
{
    public int KPIGroupId { get; set; }

    public string Name { get; set; }

    public double? Weight { get; set; }

    public bool IsSpreadKPI { get; set; }

    public int NumberKPI { get; set; }

    public bool HasKPIOptional { get; set; }

    public int NumberKPIOptional { get; set; }

    public IList<TemporaryKPIModel> KPIs { get; set; } = new List<TemporaryKPIModel>();

    public IList<TemporaryKPIModel> KPIsBySelf { get; set; } = new List<TemporaryKPIModel>();
}