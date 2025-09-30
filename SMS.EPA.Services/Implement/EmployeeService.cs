using System.Collections.Generic;
using System.Linq;
using GO.UserControl.Client.Core.Models.Role;
using SMS.EPA.Criteria.Employee;
using SMS.EPA.Models.Enitites;
using SMS.EPA.Repositories.Interfaces;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Services.Implement;

public class EmployeeService(IEmployeeRepository employeeRepository) : IEmployeeService
{
    public IList<Employee> GetEmployee(GetEmployeeCriteria criteria, IList<Role> roles)
    {
        IList<Employee> employees = employeeRepository.GetAllEmployee(criteria).ToList();
        foreach (var employee in employees)
        {
            employee.Roles = roles.Where(x => employee.Roles.Any(r => r.RoleId == x.RoleId)).ToList();
        }

        return employees;
    }

    public Employee GetEmployeeById(int employeeId, IList<Role> roles)
    {
        var criteria = new GetEmployeeByIdCriteria
        {
            EmployeeId = employeeId
        };
        var employee = employeeRepository.GetEmployeeById(criteria);

        if (employee != null)
        {
            employee.Roles = roles.Where(x => employee.Roles.Any(r => r.RoleId == x.RoleId)).ToList();
        }

        return employee;
    }
}