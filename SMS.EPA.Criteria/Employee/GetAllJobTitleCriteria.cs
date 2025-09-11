using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.Employee;

public class GetAllJobTitleCriteria : CriteriaBase
{
    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey()
    {
        return "EpaJobTitle_Get";
    }
}