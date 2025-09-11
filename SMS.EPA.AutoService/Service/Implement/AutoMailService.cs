using System.Collections.Generic;
using SMS.EPA.AutoService.Service.Interfaces;
using SMS.EPA.Core.Enumerations;
using SMS.EPA.Models.AutoService;
using SMS.EPA.Services.Implement;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.AutoService.Service.Implement;

public class AutoMailService(
    IPeriodService periodService,
    IPeriodMailService periodMailService)
    : IAutoMailService
{
    private readonly IPeriodService periodService = periodService ?? new PeriodService();
    private readonly IPeriodMailService periodMailService = periodMailService ?? new PeriodMailService();

    public AutoMailService() : this(null, null)
    {
    }

    public void CheckAndSendMail()
    {
        CheckSendNoticeAtBeginEmail();

        CheckSendReminderEmail();
    }

    private void CheckSendNoticeAtBeginEmail()
    {
        IEnumerable<EmployeeSettingToSendMail> employeeSettings = periodService.GetEmployeeSettingToSendMail(mailMode: (byte)MailMode.Start);
        periodMailService.SendReminderMail(employeeSettings, MailMode.Start);
    }

    private void CheckSendReminderEmail()
    {
        IEnumerable<EmployeeSettingToSendMail> employeeSettings = periodService.GetEmployeeSettingToSendMail(mailMode: (byte)MailMode.Reminder, isEvaluated: false);
        periodMailService.SendReminderMail(employeeSettings, MailMode.Reminder);
    }
}