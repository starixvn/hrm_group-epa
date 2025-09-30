using System.Collections.Generic;
using GO.UserControl.Client.Core.Models.Role;
using SMS.EPA.Criteria.Employee;
using SMS.EPA.Models.Enitites;

namespace SMS.EPA.Services.Interfaces;

public interface IEmployeeService
{
    IList<Employee> GetEmployee(GetEmployeeCriteria criteria, IList<Role> roles);

    Employee GetEmployeeById(int employeeId, IList<Role> roles);
}