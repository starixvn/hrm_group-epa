using Fanex.Logging;
using SMS.EPA.AutoService.Criteria;
using SMS.EPA.AutoService.Service.Interfaces;
using SMS.EPA.Models.AutoService;

namespace SMS.EPA.AutoService.Service.Implement;

public class ProbationService : BasePeriodService, IProbationService
{
    public void CheckProbationPeriod()
    {
        var employeePeriods = GetEmployeeEvaluation(new GetEmployeeProbationEvaluationCriteria());

        foreach (var employeePeriod in employeePeriods)
        {
            if (!string.IsNullOrWhiteSpace(employeePeriod.EmployeeIds) && employeePeriod.PeriodId > 0)
            {
                InsertEmployeeToPeriod(employeePeriod);
                GenerateEPA(employeePeriod, "Auto generate EPA for probation period");
                SendReminderStartEmail(employeePeriod.PeriodId, employeePeriod.EmployeeIds);
            }
        }
    }

    private void InsertEmployeeToPeriod(AutoEmployeePeriodModel employeePeriod)
    {
        var criteria = new InsertEmployeeToProbationPeriodCriteria
        {
            PeriodId = employeePeriod.PeriodId,
            EmployeeIds = employeePeriod.EmployeeIds
        };
        var commandResult = BaseService.ExecuteNonQuery(criteria);

        if (!commandResult.IsSuccessful)
        {
            LogService.Error("Auto add employee to probation period " + employeePeriod.PeriodId + ": " + commandResult.Message);
        }
    }
}