namespace SMS.EPA.Services;

using System.Collections.Generic;
using System.Linq;
using SMS.EPA.Core.Enumerations;
using SMS.EPA.Core.Services;
using SMS.EPA.Criteria.JobLevelStandard;
using SMS.EPA.Model.JobLevelStandard;

public class JobLevelStandardPeriodService(IBaseService baseService) : IJobLevelStandardPeriodService
{
    private readonly IBaseService baseService = baseService ?? new BaseService();

    public JobLevelStandardPeriodService() : this(null)
    {
    }

    public IList<PeriodEmployeeSetting> GetEmployeeSettings(GetPeriodEmployeeSettingCriteria criteria)
    {
        (var employees, var processes) = baseService.FetchMultiple<PeriodEmployeeSetting, PeriodProcessSetting>(criteria);
        foreach (var employee in employees)
        {
            employee.Employee = processes.FirstOrDefault(x => x.EmployeeId == employee.EmployeeId && x.ProcessId == (byte)JobLevelAssessmentProcess.Employee);
            employee.Evaluators = processes.Where(x => x.EmployeeId == employee.EmployeeId && x.ProcessId == (byte)JobLevelAssessmentProcess.Evaluator).ToList();
            employee.Managements = processes.Where(x => x.EmployeeId == employee.EmployeeId && x.ProcessId == (byte)JobLevelAssessmentProcess.Management).ToList();
        }

        return employees.ToList();
    }
}