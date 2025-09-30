using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.EmployeeEPA;

public class GetEPAEndEvaluationPeriodCriteria : CriteriaBase
{
    public override string GetSettingKey()
    {
        return "EpaAuto_GetEPAEndEvaluationPeriod";
    }

    public override bool IsValid()
    {
        return true;
    }
}