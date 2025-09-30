namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using Core.Attribute;
using Core.Constants;
using Core.Model;
using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Models.User;
using GO.UserControl.Client.Core.Services;
using GO.UserControl.Client.Core.Session;
using Microsoft.AspNetCore.Mvc;
using SMS.EPA.Core.Services;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.Employee;
using SMS.EPA.Models.Enitites;
using SMS.EPA.Services.Interfaces;

public class EmployeeController(
    IUserControlSession session,
    IRoleService roleService,
    IEmployeeService employeeService,
    IBaseService baseService,
    ICryptoService cryptoService,
    IUserService userService) : BaseController(session)
{
    [ApiAuthorize(FunctionKey = EPAFunctionKey.UserManagement, Policy = Policy.Read)]
    public IEnumerable<Employee> GetAllEmployee(
        string query = "",
        string teamIds = null,
        bool? isLocal = null,
        bool? isSpecialUser = null,
        bool? isDisabledUser = null,
        bool? hasMonthlyEPA = null,
        int? jobLevelId = null)
    {
        var criteria = new GetEmployeeCriteria
        {
            Query = query,
            TeamIds = teamIds,
            IsLocal = isLocal,
            IsSpecialUser = isSpecialUser,
            IsDisabledUser = isDisabledUser,
            HasMonthlyEPA = hasMonthlyEPA,
            JobLevelId = jobLevelId,
        };
        var roles = roleService.GetBySiteId(userControlSession.SiteId);
        return employeeService.GetEmployee(criteria, roles);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.UserManagement, Policy = Policy.Host)]
    public IActionResult AssignRoleToEmployee([FromBody] AssignRoleToEmployeeCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        var commandResult = baseService.ExecuteNonQuery(criteria);
        if (commandResult.IsSuccessful)
        {
            var roles = roleService.GetBySiteId(userControlSession.SiteId);
            var employee = employeeService.GetEmployeeById(criteria.EmployeeId, roles);
            var result = userService.AssignRole(new AssignRoleParam
            {
                UserId = employee.UserId,
                RoleIds = criteria.RoleIds
            });
            return Ok(result);
        }

        return Ok(commandResult);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.UserManagement, Policy = Policy.Host)]
    public CommandResult UpdateHasMonthlyEPA(UpdateHasMonthlyEPACriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        return baseService.ExecuteNonQuery(criteria);
    }

    [HttpGet]
    public string GetDefaultPassword()
    {
        return ConfigurationManagerService.GetAppSetting("PasswordToReset");
    }

    public byte GetUserPermissionOnEmployee()
    {
        return GetPermissionOnFunction(EPAFunctionKey.UserManagement);
    }
}