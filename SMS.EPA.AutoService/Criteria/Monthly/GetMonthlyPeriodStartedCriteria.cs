using Fanex.Data.Repository;

namespace SMS.EPA.AutoService.Criteria.Monthly;

public class GetMonthlyPeriodStartedCriteria : CriteriaBase
{
    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey()
    {
        return "EpaAuto_Monthly_GetPeriodStartedToday";
    }
}