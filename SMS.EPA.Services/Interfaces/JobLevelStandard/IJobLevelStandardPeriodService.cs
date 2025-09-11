using System.Collections.Generic;
using SMS.EPA.Criteria.JobLevelStandard.Period;
using SMS.EPA.Models.JobLevelStandard.Period;

namespace SMS.EPA.Services.Interfaces.JobLevelStandard;

public interface IJobLevelStandardPeriodService
{
    IList<PeriodEmployeeSetting> GetEmployeeSettings(GetPeriodEmployeeSettingCriteria criteria);
}