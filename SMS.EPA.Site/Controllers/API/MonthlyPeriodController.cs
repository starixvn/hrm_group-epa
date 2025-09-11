using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.Period;
using SMS.EPA.Criteria.Period.GenerationEvaluation;
using SMS.EPA.Models.FlowSetting;
using SMS.EPA.Models.Period;
using SMS.EPA.Services.Implement;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Site.Controllers.API;

using System;
using GO.UserControl.Client.Core.Session;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GO.UserControl.Client.Core.Enumerables;
using SMS.EPA.Core.Attribute;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Enumerations;
using SMS.EPA.Core.Model;
using SMS.EPA.Core.Services;
using SMS.EPA.Criteria;
using SMS.EPA.Services;

public class MonthlyPeriodController(
    IUserControlSession userControlSession,
    IPeriodService periodService,
    IBaseService baseService)
    : BaseController(userControlSession)
{
    [ApiAuthorize(FunctionKey = EPAFunctionKey.MonthlyPeriod, Policy = Policy.Read)]
    public IEnumerable<PeriodModel> GetMonthlyPeriod(
        DateTime fromDate,
        DateTime toDate)
    {
        var criteria = new GetMonthlyPeriodCriteria
        {
            FromDate = fromDate,
            ToDate = toDate
        };
        var periods = baseService.GetData<PeriodModel>(criteria);

        return periods;
    }

    public IEnumerable<PeriodModel> GetAllMonthlyPeriod()
    {
        var periods = baseService.GetData<PeriodModel>(new GetAllMonthlyPeriodCriteria());

        return periods;
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.MonthlyPeriod, Policy = Policy.Read)]

    public PeriodModel GetMonthlyPeriodById(int periodId)
    {
        PeriodModel period = periodService.GetPeriodById(periodId);

        return period;
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.MonthlyPeriod, Policy = Policy.Host)]
    public CommandResult CreateMonthlyPeriod(CreatePeriodCriteria criteria)
    {
        criteria.TargetTypeId = (byte)TargetType.Monthly;
        criteria.IsActive = true;
        criteria.CreatedBy = CurrentEmployeeId;
        CommandResult commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.MonthlyPeriod, Policy = Policy.Host)]
    public CommandResult UpdateBasicInfoMonthlyPeriod(UpdateBasicInformationPeriodCriteria criteria)
    {
        criteria.TargetTypeId = (byte)TargetType.Monthly;
        criteria.IsActive = true;
        criteria.ModifiedBy = CurrentEmployeeId;
        CommandResult commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.MonthlyPeriod, Policy = Policy.Read)]

    public IEnumerable<FlowModel> GetProcessSettingByPeriodId(int periodId)
    {
        var criteria = new GetProcessSettingPeriodCriteria
        {
            PeriodId = periodId
        };
        var processSettings = baseService.GetData<FlowModel>(criteria);

        return processSettings;
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.MonthlyPeriod, Policy = Policy.Host)]
    public CommandResult UpdateProcessSettingMonthlyPeriod(UpdateProcessSettingPeriodCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        CommandResult commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }


    [ApiAuthorize(FunctionKey = EPAFunctionKey.MonthlyPeriod, Policy = Policy.Host)]


    public CommandResult DeleteMonthlyPeriod(int periodId)
    {
        var criteria = new DeletePeriodCriteria
        {
            PeriodId = periodId,
            ModifiedBy = CurrentEmployeeId
        };
        CommandResult commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.MonthlyPeriod, Policy = Policy.Read)]

    public IEnumerable<PeriodEmployeeSettingModel> GetEmployeeSettingMonthlyPeriodById([FromUri] GetPeriodEmployeeSettingCriteria criteria)
    {
        IEnumerable<PeriodEmployeeSettingModel> employeeSettings = periodService.GetPeriodEmployeeSettingById(criteria);
        employeeSettings = employeeSettings.OrderBy(x => x.Employee.Group.GroupCode).ThenBy(x => x.Employee.NickName);

        return employeeSettings;
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.MonthlyPeriod, Policy = Policy.Host)]
    public CommandResult AddEmployeeToMonthlyPeriod(AddEmployeeToPeriodCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        CommandResult commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.MonthlyPeriod, Policy = Policy.Host)]
    public CommandResult UpdateEmployeeSettingOfPeriod(UpdateEmployeeSettingOfPeriodCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        CommandResult commandResult = periodService.UpdateEmployeeSettingOfPeriod(criteria);

        return commandResult;
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.MonthlyPeriod, Policy = Policy.Host)]
    public CommandResult GenerateEvaluation(AutoGenerateEvaluationCriteria criteria)
    {
        CommandResult commandResult = baseService.ExecuteNonQuery(criteria);
        if (commandResult.IsSuccessful)
        {
            periodService.SendEmailNoticeAtBegin(criteria.PeriodId);
        }

        return commandResult;
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.MonthlyPeriod, Policy = Policy.Host)]
    public CommandResult UpdateIncludeStatusEmployeePeriod(UpdateIncludeStatusEmployeePeriodCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        CommandResult commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.MonthlyPeriod, Policy = Policy.Host)]
    public bool CheckSelfProcessStarted(int periodId, string employeeIds)
    {
        var criteria = new CheckSelfProcessStartedCriteria
        {
            PeriodId = periodId,
            EmployeeIds = employeeIds
        };
        bool hasStarted = baseService.GetDataEntity<bool>(criteria);

        return hasStarted;
    }

    public byte GetUserPermissionOnMonthlyPeriod()
    {
        return GetPermissionOnFunction(EPAFunctionKey.MonthlyPeriod);
    }
}