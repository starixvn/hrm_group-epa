namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Models.Role;
using GO.UserControl.Client.Core.Session;
using Core.Attribute;
using Core.Constants;
using Core.Enumerations;
using SMS.EPA.Core.Services;
using Criteria;
using Criteria.Dashboard;
using Model;
using Model.Dashboard;
using Services;

public class DashboardController(
    IUserControlSession userControlSession,
    IBaseService baseService,
    IEmployeeService employeeService,
    IReportService reportService)
    : BaseController(userControlSession)
{
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Home, Policy = Policy.Read)]
    public DashboardModel GetDashboard()
    {
        DashboardModel dashboard = reportService.GetDashboard(CurrentEmployeeId);
        dashboard.Employee = employeeService.GetEmployeeById(CurrentEmployeeId, new List<Role>());

        return dashboard;
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.Home, Policy = Policy.Read)]
    public ToDoListViewModel GetToDoList()
    {
        return new ToDoListViewModel
        {
            EPAToDoList = GetEPAToDoList(),
            JLSToDoList = GetJobLevelStandardToDoList()
        };
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.Home, Policy = Policy.Read)]
    public IList<object> GetAnnouncement()
    {
        var result = new List<object>();
        var criteria = new GetAnnouncementCriteria
        {
            EmployeeId = CurrentEmployeeId
        };
        var settings = baseService.GetData<JobLevelStandardProcessSetting>(criteria);
        foreach (var setting in settings)
        {
            var fromDate = setting.FromDate.Date.ToString(Constants.FormatDateTimeEmail);
            var toDate = setting.ToDate.AddDays(1).AddSeconds(-1).ToString(Constants.FormatDateTimeEmail);
            switch (setting.ProcessId)
            {
                case (byte)JobLevelAssessmentProcess.Evaluator:
                    result.Add(new
                    {
                        Announcement = string.Format("{0} Assessment is ready for key-in data. You are given a period from {1} until {2} to complete this Assessment.",
                            setting.PeriodName,
                            fromDate,
                            toDate),
                        PeriodId = setting.PeriodId,
                    });
                    break;

                case (byte)JobLevelAssessmentProcess.Employee:
                    result.Add(new
                    {
                        Announcement = string.Format("{0} Assessment is ready for your Verification. You are given a period from {1} until {2} to complete the Verification.",
                            setting.PeriodName,
                            fromDate,
                            toDate),
                        PeriodId = setting.PeriodId,
                    });
                    break;

                case (byte)JobLevelAssessmentProcess.Management:
                    result.Add(new
                    {
                        Announcement = string.Format("{0} Assessment is ready for your Approval. You are given a period from {1} until {2} to complete the Approval.",
                            setting.PeriodName,
                            fromDate,
                            toDate),
                        PeriodId = setting.PeriodId,
                    });
                    break;

                default:
                    // nothing
                    break;
            }
        }

        return result;
    }

    private EPAToDoListViewModel GetEPAToDoList()
    {
        var criteria = new GetToDoListCriteria
        {
            EmployeeId = CurrentEmployeeId
        };
        return new EPAToDoListViewModel
        {
            ToDoList = baseService.GetData<EPAToDoList>(criteria)
        };
    }

    private JobLevelStandardDashboardViewModel GetJobLevelStandardToDoList()
    {
        var criteria = new GetJobLevelStandardDashboardCriteria
        {
            EmployeeId = CurrentEmployeeId
        };
        return new JobLevelStandardDashboardViewModel
        {
            ToDoList = baseService.GetData<JobLevelStandardDashboard>(criteria)
        };
    }
}