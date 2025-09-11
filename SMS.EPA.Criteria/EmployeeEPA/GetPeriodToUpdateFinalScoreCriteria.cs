using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.EmployeeEPA;

public class GetPeriodToUpdateFinalScoreCriteria : CriteriaBase
{
    public override string GetSettingKey()
    {
        return "EPA_GetPeriodToUpdateFinalScore";
    }

    public override bool IsValid()
    {
        return true;
    }
}