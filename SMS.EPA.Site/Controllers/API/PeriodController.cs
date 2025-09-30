using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.Period;
using SMS.EPA.Criteria.Period.GenerationEvaluation;
using SMS.EPA.Models.FlowSetting;
using SMS.EPA.Models.Period;
using SMS.EPA.Services.Implement;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Site.Controllers.API;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Attribute;
using Core.Constants;
using Core.Enumerations;
using Core.Model;
using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Session;
using Microsoft.AspNetCore.Mvc;
using SMS.EPA.Core.Services;

public class PeriodController(
    IUserControlSession userControlSession,
    IPeriodService periodService,
    IBaseService baseService,
    IEPAProcessService processService,
    IEmployeeEPAService epaService) : BaseController(userControlSession)
{
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Period, Policy = Policy.Read)]
    public IEnumerable<PeriodModel> GetPeriod(int? targetTypeId = null, bool? isActive = true)
    {
        var criteria = new GetPeriodCriteria
        {
            TargetTypeId = targetTypeId,
            IsActive = isActive
        };

        var periods = periodService.GetPeriod(criteria);
        var probationPeriods = periods.Where(x => x.TargetType.TargetTypeId == (byte)TargetType.Probation);
        IEnumerable<PeriodModel> promotionPeriods = periods.Where(x => x.TargetType.TargetTypeId == (byte)TargetType.Promotion)
            .OrderByDescending(x => x.StartDate);
        IEnumerable<PeriodModel> semiPeriods = periods.Where(x => x.TargetType.TargetTypeId == (byte)TargetType.SemiAnnual)
            .OrderByDescending(x => x.StartDate);

        IEnumerable<PeriodModel> periodList = new List<PeriodModel>();

        if (probationPeriods.Any())
        {
            periodList = periodList.Concat(probationPeriods);
        }

        if (promotionPeriods.Any())
        {
            periodList = periodList.Concat(promotionPeriods);
        }

        if (semiPeriods.Any())
        {
            periodList = periodList.Concat(semiPeriods);
        }

        return periodList;
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Period, Policy = Policy.Host)]
    public CommandResult CreatePeriod(CreatePeriodCriteria criteria)
    {
        criteria.CreatedBy = CurrentEmployeeId;
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Period, Policy = Policy.Host)]
    public CommandResult UpdateBasicInformationPeriod(UpdateBasicInformationPeriodCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        var commandResult = baseService.ExecuteNonQuery(criteria);
        if (commandResult.IsSuccessful)
        {
            Task.Factory.StartNew(() =>
            {
                processService.UpdateTotalScoreOfProcess(criteria.PeriodId);
                epaService.GetDataAndUpdateFinalScore(criteria.ModifiedBy, criteria.PeriodId);
            });
        }

        return commandResult;
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.Period, Policy = Policy.Read)]
    public PeriodModel GetPeriodById(int periodId)
    {
        return periodService.GetPeriodById(periodId);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Period, Policy = Policy.Host)]
    public CommandResult UpdateStatusPeriod(UpdateStatusPeriodCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.Period, Policy = Policy.Host)]
    public CommandResult DeletePeriod(int periodId)
    {
        var criteria = new DeletePeriodCriteria
        {
            PeriodId = periodId,
            ModifiedBy = CurrentEmployeeId
        };
        return baseService.ExecuteNonQuery(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.Period, Policy = Policy.Read)]
    public IEnumerable<PeriodEmployeeSettingModel> GetPeriodEmployeeSettingById(GetPeriodEmployeeSettingCriteria criteria)
    {
        var employeeSettings = periodService.GetPeriodEmployeeSettingById(criteria);
        if (employeeSettings.Any())
        {
            var employeeSetting = employeeSettings.FirstOrDefault(x => x.TargetTemplate != null);
            var targetTypeId = GetTargetTypeId(employeeSetting);
            switch (targetTypeId)
            {
                case (byte)TargetType.Probation:
                case (byte)TargetType.Promotion:
                    employeeSettings = employeeSettings.OrderByDescending(x => x.SelfEvaluators.FirstOrDefault()?.StartDate ?? DateTimeOffset.MaxValue);
                    break;

                case (byte)TargetType.Monthly:
                case (byte)TargetType.SemiAnnual:
                    employeeSettings = employeeSettings.OrderBy(x => x.Employee.Group != null ? x.Employee.Group.GroupCode : string.Empty)
                        .ThenBy(x => x.Employee.NickName);
                    break;

                default:
                    // do nothing
                    break;
            }
        }

        return employeeSettings;
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Period, Policy = Policy.Host)]
    public CommandResult AddEmployeToPeriod(AddEmployeeToPeriodCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Period, Policy = Policy.Host)]
    public CommandResult UpdateEmployeeSettingOfPeriod(UpdateEmployeeSettingOfPeriodCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        var commandResult = periodService.UpdateEmployeeSettingOfPeriod(criteria);
        if (commandResult.IsSuccessful)
        {
            Task.Factory.StartNew(() =>
            {
                processService.UpdateTotalScoreOfProcess(criteria.PeriodId, criteria.EmployeeIds);
                epaService.GetDataAndUpdateFinalScore(criteria.ModifiedBy, criteria.PeriodId, criteria.EmployeeIds);
            });
        }

        return commandResult;
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Period, Policy = Policy.Host)]
    public CommandResult UpdateIncludeStatusEmployeePeriod(UpdateIncludeStatusEmployeePeriodCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Period, Policy = Policy.Host)]
    public CommandResult GenerateEvaluation(AutoGenerateEvaluationCriteria criteria)
    {
        var extendedQuetionEnabled = ConfigurationManagerService.GetAppSetting("ProbationExtendedQuestionEnabled");
        criteria.ExtendedQuestionEnabled = extendedQuetionEnabled.Equals("true", StringComparison.OrdinalIgnoreCase);

        var commandResult = baseService.ExecuteNonQuery(criteria);
        if (commandResult.IsSuccessful)
        {
            periodService.SendEmailNoticeAtBegin(criteria.PeriodId);
        }

        return commandResult;
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.Period, Policy = Policy.Read)]
    public IEnumerable<FlowModel> GetProcessSettingByPeriodId(int periodId)
    {
        var criteria = new GetProcessSettingPeriodCriteria
        {
            PeriodId = periodId
        };
        return baseService.GetData<FlowModel>(criteria);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Period, Policy = Policy.Host)]
    public CommandResult UpdatePeriodProcessSetting(UpdateProcessSettingPeriodCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    public bool CheckSelfProcessStarted(int periodId, string employeeIds)
    {
        var criteria = new CheckSelfProcessStartedCriteria
        {
            PeriodId = periodId,
            EmployeeIds = employeeIds
        };
        return baseService.GetDataEntity<bool>(criteria);
    }

    public byte GetUserPermissionOnPeriod()
    {
        return GetPermissionOnFunction(EPAFunctionKey.Period);
    }

    private static byte GetTargetTypeId(PeriodEmployeeSettingModel employeeSetting)
    {
        return employeeSetting?.TargetTemplate.TargetTypeId ?? 0;
    }
}