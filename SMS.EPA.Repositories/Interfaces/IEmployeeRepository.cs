using System.Collections.Generic;
using Fanex.Data.Repository;
using SMS.EPA.Models.Enitites;

namespace SMS.EPA.Repositories.Interfaces;

public interface IEmployeeRepository
{
    IEnumerable<Employee> GetAllEmployee(ICriteria criteria);

    Employee GetEmployeeById(ICriteria criteria);
}