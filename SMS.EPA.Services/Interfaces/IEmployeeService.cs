using System.Collections.Generic;
using GO.UserControl.Client.Core.Models.Role;
using SMS.EPA.Core.Model;
using SMS.EPA.Criteria.Employee;
using SMS.EPA.Models.Enitites;

namespace SMS.EPA.Services.Interfaces;

public interface IEmployeeService
{
    bool ValidatePassword(string enterPassword, string employeePassword);

    IList<Employee> GetEmployee(GetEmployeeCriteria criteria, IList<Role> roles);

    Employee GetEmployeeById(int employeeId, IList<Role> roles);

    CommandResult CreateSpecialEmployee(CreateSpecialEmployeeCriteria criteria);

    CommandResult ChangePassword(ChangePasswordCriteria criteria);

    CommandResult ResetPassword(ResetPasswordCriteria criteria);

    string GetPasswordByEmployeeId(int employeeId);
}