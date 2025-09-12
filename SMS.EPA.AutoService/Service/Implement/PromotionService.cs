using SMS.EPA.AutoService.Criteria;
using SMS.EPA.AutoService.Service.Interfaces;

namespace SMS.EPA.AutoService.Service.Implement;

public class PromotionService : BasePeriodService, IPromotionService
{
    public void CheckPromotionPeriod()
    {
        var employeePeriods = GetEmployeeEvaluation(new GetEmployeePromotionEvaluationCriteria());

        foreach (var employeePeriod in employeePeriods)
        {
            if (!string.IsNullOrWhiteSpace(employeePeriod.EmployeeIds) && employeePeriod.PeriodId > 0)
            {
                GenerateEPA(employeePeriod, "Auto generate EPA for promotion period");
                SendReminderStartEmail(employeePeriod.PeriodId, employeePeriod.EmployeeIds);
            }
        }
    }
}