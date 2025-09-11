using System.Collections.Generic;
using SMS.EPA.AutoService.Criteria;
using SMS.EPA.AutoService.Service.Interfaces;
using SMS.EPA.Models.AutoService;

namespace SMS.EPA.AutoService.Service.Implement;

public class PromotionService : BasePeriodService, IPromotionService
{
    public void CheckPromotionPeriod()
    {
        IEnumerable<AutoEmployeePeriodModel> employeePeriods = GetEmployeeEvaluation(new GetEmployeePromotionEvaluationCriteria());

        foreach (AutoEmployeePeriodModel employeePeriod in employeePeriods)
        {
            if (!string.IsNullOrWhiteSpace(employeePeriod.EmployeeIds) && employeePeriod.PeriodId > 0)
            {
                GenerateEPA(employeePeriod, "Auto generate EPA for promotion period");
                SendReminderStartEmail(employeePeriod.PeriodId, employeePeriod.EmployeeIds);
            }
        }
    }
}