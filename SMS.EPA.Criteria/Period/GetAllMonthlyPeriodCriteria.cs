using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.Period;

public class GetAllMonthlyPeriodCriteria : CriteriaBase
{
    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey()
    {
        return "EpaMonthlyPeriod_GetAll";
    }
}