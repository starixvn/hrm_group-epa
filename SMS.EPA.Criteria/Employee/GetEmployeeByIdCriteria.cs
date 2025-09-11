using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.Employee;

public class GetEmployeeByIdCriteria : CriteriaBase
{
    public int EmployeeId { get; set; }

    public override string GetSettingKey() => "EpaEmployee_GetById";

    public override bool IsValid() => EmployeeId > 0;
}