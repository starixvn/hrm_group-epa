using SMS.EPA.AutoService.Criteria;
using SMS.EPA.AutoService.Service.Interfaces;

namespace SMS.EPA.AutoService.Service.Implement;

public class SemiAnnualService : BasePeriodService, ISemiAnnualService
{
    public void CheckSemiAnnualPeriod()
    {
        var semiAnnualPeriods = BaseService.GetData<int>(new GetSemiAnnualPeriodStartedCriteria());
        foreach (var periodId in semiAnnualPeriods)
        {
            InsertEmployeeToPeriod(periodId, new InsertEmployeeToSemiAnnualPeriodCriteria(periodId), "Auto add employee to semi annual period");
            GenerateEPA(periodId, "Auto generate EPA for semi annual period");
            SendReminderStartEmail(periodId);
        }
    }
}