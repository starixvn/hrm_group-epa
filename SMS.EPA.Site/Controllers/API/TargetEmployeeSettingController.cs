using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.TargetEmployeeSetting;
using SMS.EPA.Models.TargetSetting;

namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using Core.Attribute;
using Core.Constants;
using Core.Model;
using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Session;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

public class TargetEmployeeSettingController(
    IUserControlSession userControlSession,
    ITargetEmployeeSettingService targetEmployeeSettingService,
    IBaseService baseService) : BaseController(userControlSession)
{
    [ApiAuthorize(FunctionKey = EPAFunctionKey.TargetEmployeeSetting, Policy = Policy.Read)]
    public IList<TargetEmployeeSetting> GetTargetEmployeeSetting(
        string teamIds = null,
        int? employeeId = null,
        int? targetTypeId = null,
        bool showAll = false)
    {
        var criteria = new GetAllTargetEmployeeSettingCriteria
        {
            TeamIds = teamIds,
            EmployeeId = employeeId,
            TargetTypeId = targetTypeId,
            ShowAll = showAll
        };

        return targetEmployeeSettingService.GetTargetEmployeeSetting(criteria);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.TargetEmployeeSetting, Policy = Policy.Host)]
    public CommandResult AssignTargetTemplateToEmployee(AssignTargetTemplateToEmployeeCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.TargetEmployeeSetting, Policy = Policy.Host)]
    public CommandResult DeleteTargetEmployeeSetting(int targetTemplateId, int employeeId)
    {
        var criteria = new DeleteTargetEmployeeSettingCriteria
        {
            TargetTemplateId = targetTemplateId,
            EmployeeId = employeeId,
            ModifiedBy = CurrentEmployeeId
        };
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.TargetEmployeeSetting, Policy = Policy.Read)]
    public IList<TargetDepartmentSetting> GetTargetDepartmentSetting(
        string departmentIds = null,
        int? targetTypeId = null,
        bool showAll = false)
    {
        var criteria = new GetTargetDepartmentSettingCriteria
        {
            DepartmentIds = departmentIds,
            TargetTypeId = targetTypeId,
            ShowAll = showAll
        };
        var targetDepartments = targetEmployeeSettingService.GetTargetDepartmentSetting(criteria);

        return targetDepartments;
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.TargetEmployeeSetting, Policy = Policy.Host)]
    public CommandResult AssignTargetTemplateToDepartment(TargetSettingAssignModel model)
    {
        var commandResult = new CommandResult();

        if (model != null)
        {
            var criteria = new AssignTargetDepartmentSettingCriteria
            {
                TargetTemplateId = model.TargetTemplateId,
                Departments = model.ConvertToXml(),
                ModifiedBy = CurrentEmployeeId
            };
            commandResult = baseService.ExecuteNonQuery(criteria);
        }

        return commandResult;
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.TargetEmployeeSetting, Policy = Policy.Host)]
    public CommandResult DeleteTargetDepartmentSetting(
        int targetTemplateId,
        int departmentId,
        bool isGroup)
    {
        var criteria = new DeleteTargetDepartmentSettingCriteria
        {
            TargetTemplateId = targetTemplateId,
            DepartmentId = departmentId,
            IsGroup = isGroup,
            ModifiedBy = CurrentEmployeeId
        };
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    public byte GetUserPermissionOnTargetEmployeeSetting()
    {
        return GetPermissionOnFunction(EPAFunctionKey.TargetEmployeeSetting);
    }
}