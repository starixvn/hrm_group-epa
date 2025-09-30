using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.Employee;

public class GetAllGroupCriteria : CriteriaBase
{
    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey()
    {
        return "EpaGroup_Get";
    }
}