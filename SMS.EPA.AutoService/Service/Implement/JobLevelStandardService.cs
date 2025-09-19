using System;
using System.IO;
using SMS.EPA.AutoService.Criteria.JobLevelStandard;
using SMS.EPA.AutoService.Service.Interfaces;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Enumerations;
using SMS.EPA.Core.Services;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Models.AutoService;
using SMS.EPA.Models.Email;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.AutoService.Service.Implement;

public class JobLevelStandardService(IBaseService baseService, IBaseMailService mailService) : IJobLevelStandardService
{
    private const string StartMailTemplateToEmployee = "JobLevelStandard\\StartToEmployee.cshtml";
    private const string StartMailTemplateToEvaluator = "JobLevelStandard\\StartToEvaluator.cshtml";
    private const string StartMailTemplateToManagement = "JobLevelStandard\\StartToManagement.cshtml";

    public JobLevelStandardService() : this(null, null)
    {
    }

    public void SendReminderEmail()
    {
        var sender = ConfigurationManagerService.GetAppSetting("Email.SenderName");
        var siteurl = ConfigurationManagerService.GetAppSetting("url");

        var criteria = new GetJobLevelStandardPeriodEmployeeSettingToSendMailCriteria();
        var employeeSettings = baseService.GetData<JobLevelStandardProcessSetting>(criteria);
        foreach (var setting in employeeSettings)
        {
            var emailTemplatePath = GetEmailTemplatePath(setting.ProcessId);
            var bodyTemplate = mailService.ReadTextFile(emailTemplatePath);

            var model = new MailModel
            {
                Subject = GetSubject(setting),
                Body = string.Format(
                    bodyTemplate,
                    setting.Nickname,
                    sender,
                    setting.PeriodName,
                    siteurl,
                    setting.FromDate.Date.ToString(Constants.FormatDateTimeEmail),
                    setting.ToDate.AddDays(1).AddSeconds(-1).ToString(Constants.FormatDateTimeEmail)),
                ReceiverEmail = setting.Email
            };

            mailService.SendEmail(model);
        }
    }

    private static string GetEmailTemplatePath(byte processId)
    {
        string emailTemplatePath = processId switch
        {
            (byte)JobLevelAssessmentProcess.Employee => StartMailTemplateToEmployee,
            (byte)JobLevelAssessmentProcess.Evaluator => StartMailTemplateToEvaluator,
            (byte)JobLevelAssessmentProcess.Management => StartMailTemplateToManagement,
            _ => string.Empty
        };

        var assemblyPath = AppDomain.CurrentDomain.BaseDirectory;
        return Path.Combine(assemblyPath, ConfigurationManagerService.GetAppSetting("RelativeEmailPath") + emailTemplatePath);
    }

    private static string GetSubject(JobLevelStandardProcessSetting setting)
    {
        var fromDate = setting.FromDate.Date.ToString(Constants.FormatDateTimeEmail);
        var toDate = setting.ToDate.AddDays(1).AddSeconds(-1).ToString(Constants.FormatDateTimeEmail);
        var subject = setting.ProcessId switch
        {
            (byte)JobLevelAssessmentProcess.Employee =>
                $"{setting.PeriodName} Assessment Verification from {fromDate} to {toDate}",
            (byte)JobLevelAssessmentProcess.Evaluator => $"{setting.PeriodName} Assessment from {fromDate} to {toDate}",
            (byte)JobLevelAssessmentProcess.Management =>
                $"{setting.PeriodName} Assessment Approval from {fromDate} to {toDate}",
            _ => string.Empty
        };

        return subject;
    }
}