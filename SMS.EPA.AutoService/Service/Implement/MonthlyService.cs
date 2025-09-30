using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.Logging;
using SMS.EPA.AutoService.Criteria.Monthly;
using SMS.EPA.AutoService.Service.Interfaces;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Enumerations;
using SMS.EPA.Criteria.Period;
using SMS.EPA.Models.FlowSetting;

namespace SMS.EPA.AutoService.Service.Implement;

public class MonthlyService : BasePeriodService, IMonthlyService
{
    public void CheckMonthlyPeriod()
    {
        var monthlyPeriods = BaseService.GetData<int>(new GetMonthlyPeriodStartedCriteria());
        foreach (var periodId in monthlyPeriods)
        {
            InsertEmployeeToPeriod(periodId, new InsertEmployeeToMonthlyPeriodCriteria(periodId), "Auto add employee to monthly period");
            GenerateEPA(periodId, "Auto generate EPA for monthly period");
            SendReminderStartEmail(periodId);
        }
    }

    public void CreateMonthlyPeriod()
    {
        var processSettings = GetEvaluationGlobalSetting();

        if (processSettings != null && processSettings.Any())
        {
            var today = DateTimeOffset.Now.Date;
            var startDayCreateMonthlyPeriod = AppSetting.StartDayCreateMonthlyPeriod;

            if (today.Day.Equals(startDayCreateMonthlyPeriod))
            {
                var criteria = new AutoCreateMonthlyPeriodCriteria
                {
                    Name = today.AddMonths(1).ToString(Constants.FormatMonthlyPeriodName),
                    StartDate = today,
                    EndDate = today.AddDays(processSettings.Sum(p => p.Duration))
                };
                var commandResult = BaseService.ExecuteNonQuery(criteria);

                if (!commandResult.IsSuccessful)
                {
                    LogService.Error($"Error create monthly period: {commandResult.Message}");
                }
            }
        }
    }

    private IEnumerable<FlowModel> GetEvaluationGlobalSetting()
    {
        var criteria = new GetEvaluationGlobalSettingByTargetTypeIdCriteria
        {
            TargetTypeId = (byte)TargetType.Monthly
        };
        var processSettings = BaseService.GetData<FlowModel>(criteria);

        return processSettings;
    }
}