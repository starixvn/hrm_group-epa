using SMS.EPA.Core.Enumerations;
using SMS.EPA.Models.AutoService;
using SMS.EPA.Models.EmployeeEPA;

namespace SMS.EPA.Services.Interfaces;

public interface IMailService
{
    void SendMailReminder(string emailTemplatePath, EmployeeSettingToSendMail employeeSetting, MailMode mailMode);

    void SendMailToNotifyChangeSetting(string emailTemplatePath, EmployeeSettingToSendMail employeeSetting);

    void SendMailToSelfEndEvaluation(string emailTemplatePath, EPAModel evaluationInfo, float? averagePerformanceScore, float disciplineScore);

    void SendMailEndToHumanResourcesByEmployee(string emailTemplatePath, EPAModel evaluationInfo, EPAProcessModel humanResource);

    void SendMailEndToHumanResourceByPeriod(string emailTemplatePath, EmployeeSettingToSendMail employeeSetting);
}