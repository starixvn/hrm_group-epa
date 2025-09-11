using System.Collections.Generic;
using SMS.EPA.Core.Model;
using SMS.EPA.Criteria.Period;
using SMS.EPA.Models.AutoService;
using SMS.EPA.Models.Period;

namespace SMS.EPA.Services.Interfaces;

public interface IPeriodService
{
    IEnumerable<PeriodModel> GetPeriod(GetPeriodCriteria criteria);

    PeriodModel GetPeriodById(int periodId);

    IEnumerable<PeriodEmployeeSettingModel> GetPeriodEmployeeSettingById(GetPeriodEmployeeSettingCriteria criteria);

    CommandResult UpdateEmployeeSettingOfPeriod(UpdateEmployeeSettingOfPeriodCriteria criteria);

    void SendEmailNoticeAtBegin(int? periodId = null);

    IEnumerable<EmployeeSettingToSendMail> GetEmployeeSettingToSendMail(
        int? periodId = null,
        byte? processId = null,
        byte? mailMode = null,
        string employeeIds = null,
        bool? isEvaluated = null);
}