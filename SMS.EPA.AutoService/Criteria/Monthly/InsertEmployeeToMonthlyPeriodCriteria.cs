using Fanex.Data.Repository;

namespace SMS.EPA.AutoService.Criteria.Monthly;

public class InsertEmployeeToMonthlyPeriodCriteria : CriteriaBase
{
    public InsertEmployeeToMonthlyPeriodCriteria(int periodId = 0)
    {
        if (periodId > 0)
        {
            PeriodId = periodId;
        }
    }

    public int PeriodId { get; set; }

    public override string GetSettingKey()
    {
        return "EpaAuto_Monthly_AddEmployeeToPeriod";
    }

    public override bool IsValid()
    {
        return PeriodId > 0;
    }
}