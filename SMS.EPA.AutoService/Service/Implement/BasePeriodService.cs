namespace SMS.EPA.AutoService.Service;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Data.Repository;
using Fanex.Logging;
using SMS.EPA.Core.Enumerations;
using SMS.EPA.Core.Model;
using SMS.EPA.Core.Services;
using SMS.EPA.Criteria;
using SMS.EPA.Model;
using SMS.EPA.Model.AutoService;
using SMS.EPA.Services;

public class BasePeriodService(
    IPeriodService periodService,
    IPeriodMailService periodMailService,
    IBaseService baseService)
{
    protected readonly IPeriodMailService PeriodMailService = periodMailService ?? new PeriodMailService();
    protected readonly IPeriodService PeriodService = periodService ?? new PeriodService();
    protected readonly IBaseService BaseService = baseService ?? new BaseService();

    public BasePeriodService() : this(null, null, null)
    {
    }

    protected void InsertEmployeeToPeriod(int periodId, ICriteria criteria, string messageExplanation)
    {
        var commandResult = new CommandResult();
        if (criteria.IsValid())
        {
            commandResult = BaseService.ExecuteNonQuery(criteria);
        }

        if (!commandResult.IsSuccessful)
        {
            LogService.Error(string.Format("{0} {1}: {2}", messageExplanation, periodId, commandResult.Message));
        }
    }

    protected void GenerateEPA(int periodId, string messageExplanation)
    {
        var criteria = new AutoGenerateEvaluationCriteria
        {
            PeriodId = periodId
        };
        CommandResult commandResult = BaseService.ExecuteNonQuery(criteria);

        if (!commandResult.IsSuccessful)
        {
            LogService.Error(string.Format("{0} {1}: {2}", messageExplanation, periodId, commandResult.Message));
        }
    }

    protected void GenerateEPA(AutoEmployeePeriodModel employeePeriod, string messageExplanation)
    {
        var criteria = new AutoGenerateEvaluationCriteria
        {
            PeriodId = employeePeriod.PeriodId,
            EmployeeIds = employeePeriod.EmployeeIds,
            ExtendedQuestionEnabled = AppSetting.ProbationExtendedQuestionEnabled
        };
        CommandResult commandResult = BaseService.ExecuteNonQuery(criteria);

        if (!commandResult.IsSuccessful)
        {
            LogService.Error(string.Format("{0} {1}: {2}", messageExplanation, employeePeriod.PeriodId, commandResult.Message));
        }
    }

    protected IEnumerable<AutoEmployeePeriodModel> GetEmployeeEvaluation(ICriteria criteria)
    {
        IEnumerable<AutoEmployeePeriodModel> employeePeriods = new List<AutoEmployeePeriodModel>();

        if (criteria.IsValid())
        {
            employeePeriods = BaseService.GetData<AutoEmployeePeriodModel>(criteria);
            employeePeriods = employeePeriods.GroupBy(x => x.PeriodId).Select(x => new AutoEmployeePeriodModel
            {
                PeriodId = x.Key,
                EmployeeIds = string.Join(",", x.Select(i => i.EmployeeId.ToString()))
            });
        }

        return employeePeriods;
    }

    protected void SendReminderStartEmail(int periodId, string employeeIds = null)
    {
        if (AppSetting.SendMailAllWhenPeriodStarted)
        {
            Task.Factory.StartNew(() =>
            {
                IEnumerable<EmployeeSettingToSendMail> employeeSettings =
                    PeriodService.GetEmployeeSettingToSendMail(periodId, mailMode: (byte)MailMode.Start, employeeIds: employeeIds);
                if (employeeSettings != null && employeeSettings.Any())
                {
                    PeriodMailService.SendReminderMail(employeeSettings, MailMode.Start);
                }
            });
        }
    }
}