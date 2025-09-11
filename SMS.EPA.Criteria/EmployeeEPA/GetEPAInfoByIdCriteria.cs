using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.EmployeeEPA;

public class GetEPAInfoByIdCriteria : CriteriaBase
{
    public int EPAId { get; set; }

    public override bool IsValid()
    {
        return EPAId > 0;
    }

    public override string GetSettingKey()
    {
        return "EPA_GetInfoById";
    }
}