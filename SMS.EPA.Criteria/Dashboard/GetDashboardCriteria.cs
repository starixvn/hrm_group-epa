using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.Dashboard;

public class GetDashboardCriteria : CriteriaBase
{
    public int EmployeeId { get; set; }

    public override string GetSettingKey() => "Epa_Dashboard";

    public override bool IsValid() => EmployeeId > 0;
}