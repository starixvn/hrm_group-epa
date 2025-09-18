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

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.UserManagement, Policy = Policy.Host)]
    public IActionResult CreateSpecialEmployee(CreateSpecialEmployeeCriteria criteria)
    {
        var password = criteria.Password;
        criteria.CreatedBy = CurrentEmployeeId;
        var commandResult = employeeService.CreateSpecialEmployee(criteria);
        if (commandResult.IsSuccessful)
        {
            var token = cryptoService.GenerateRijndaelKeyIV();
            var keyiv = token.Split(':');
            var rijndaelKey = keyiv[0];
            var rijndaelIV = keyiv[1];
            var result = userService.Create(new CreateUserParam
            {
                RefId = criteria.EmployeeId,
                Username = criteria.Username,
                Nickname = criteria.NickName,
                FullName = criteria.FullName,
                BCryptPassword = cryptoService.Encrypt(password, rijndaelKey, rijndaelIV),
                Token = token
            });
            if (result.IsSuccessful)
            {
                baseService.ExecuteNonQuery(new UpdateUserIdCommand
                {
                    EmployeeId = criteria.EmployeeId,
                    UserId = result.ObjectId
                });
            }

            return Ok(result);
        }

        return Ok(commandResult);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.UserManagement, Policy = Policy.Host)]
    public IActionResult UpdateSpecialEmployee(UpdateSpecialEmployeeCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        var commandResult = baseService.ExecuteNonQuery(criteria);
        if (commandResult.IsSuccessful)
        {
            var roles = roleService.GetBySiteId(userControlSession.SiteId);
            var employee = employeeService.GetEmployeeById(criteria.EmployeeId, roles);
            var result = userService.UpdateInfo(new UpdateUserInfoParam
            {
                RefId = criteria.EmployeeId,
                UserId = employee.UserId,
                Code = employee.Code,
                Nickname = criteria.NickName,
                FullName = criteria.FullName,
                Email = criteria.Email,
                IsActive = criteria.Enabled
            });
            return Ok(result);
        }

        return Ok(commandResult);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.UserManagement, Policy = Policy.Host)]
    public CommandResult UpdateJobLevel(UpdateJobLevelCriteria criteria)
    {
        return baseService.ExecuteNonQuery(criteria);
    }

    [HttpPut]
    public IActionResult ChangePassword(ChangePasswordCriteria criteria)
    {
        var password = criteria.NewPassword;
        criteria.EmployeeId = CurrentEmployeeId;
        criteria.ModifiedBy = CurrentEmployeeId;
        var commandResult = employeeService.ChangePassword(criteria);
        if (commandResult.IsSuccessful)
        {
            var token = cryptoService.GenerateRijndaelKeyIV();
            var keyiv = token.Split(':');
            var rijndaelKey = keyiv[0];
            var rijndaelIV = keyiv[1];
            var result = userService.UpdatePassword(new UpdatePasswordParam
            {
                OldPassword = cryptoService.Encrypt(criteria.OldPassword, rijndaelKey, rijndaelIV),
                Password = cryptoService.Encrypt(password, rijndaelKey, rijndaelIV),
                Token = token
            });
            return Ok(result);
        }

        return Ok(commandResult);
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

    [HttpGet]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.UserManagement, Policy = Policy.Host)]
    public IActionResult ResetPassword(int employeeId)
    {
        var newPassword = ConfigurationManagerService.GetAppSetting("PasswordToReset");
        var criteria = new ResetPasswordCriteria
        {
            EmployeeId = employeeId,
            NewPassword = newPassword,
            ModifiedBy = CurrentEmployeeId
        };
        var commandResult = employeeService.ResetPassword(criteria);
        if (commandResult.IsSuccessful)
        {
            var roles = roleService.GetBySiteId(userControlSession.SiteId);
            var employee = employeeService.GetEmployeeById(employeeId, roles);
            var token = cryptoService.GenerateRijndaelKeyIV();
            var keyiv = token.Split(':');
            var rijndaelKey = keyiv[0];
            var rijndaelIV = keyiv[1];
            var result = userService.ResetPassword(new UpdatePasswordParam
            {
                UserId = employee.UserId,
                Password = cryptoService.Encrypt(newPassword, rijndaelKey, rijndaelIV),
                Token = token
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