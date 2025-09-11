using System.Collections.Generic;
using System.Linq;
using SMS.EPA.Core.Enumerations;
using SMS.EPA.Core.Services;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.JobLevelStandard.Period;
using SMS.EPA.Models.JobLevelStandard.Period;
using SMS.EPA.Services.Interfaces.JobLevelStandard;

namespace SMS.EPA.Services.Implement.JobLevelStandard;

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