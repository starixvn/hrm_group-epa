namespace SMS.EPA.AutoService.Service;

using System.Collections.Generic;
using Core.Enumerations;
using SMS.EPA.Model.AutoService;
using SMS.EPA.Services;

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