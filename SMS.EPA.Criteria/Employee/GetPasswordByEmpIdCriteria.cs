using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.Employee;

public class GetPasswordByEmpIdCriteria : CriteriaBase
{
    public int EmployeeId { get; set; }

    public override bool IsValid()
    {
        return EmployeeId > 0;
    }

    public override string GetSettingKey()
    {
        return "EpaEmployee_GetPasswordByEmpId";
    }
}