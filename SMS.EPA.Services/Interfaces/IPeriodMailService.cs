using System.Collections.Generic;
using SMS.EPA.Core.Enumerations;
using SMS.EPA.Models.AutoService;
using SMS.EPA.Models.EmployeeEPA;

namespace SMS.EPA.Services.Interfaces;

public interface IPeriodMailService
{
    void SendReminderMail(IEnumerable<EmployeeSettingToSendMail> employeeSettings, MailMode mailMode);

    void SendEndEvaluationMailToSelf(EPAModel evaluationInfo, float? averagePerformanceScore, float disciplineScore);

    void SendEndEvaluationMailToHumanResourceByEmployee(EPAModel evaluationInfo);

    void SendEndEvaluationMailToHumanResourceByPeriod(IEnumerable<EmployeeSettingToSendMail> employeeSettings);

    void SendNotifyMail(IEnumerable<EmployeeSettingToSendMail> employeeSettings);
}