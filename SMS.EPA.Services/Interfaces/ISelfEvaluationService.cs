using System.Collections.Generic;
using SMS.EPA.Criteria.EmployeeEPA;
using SMS.EPA.Models.EmployeeEPA;

namespace SMS.EPA.Services.Interfaces;

public interface ISelfEvaluationService
{
    IEnumerable<EPAModel> GetEPAByEmployee(GetEPAByEmployeeCriteria criteria);

    EPAModel GetEPAInfoByEmployee(GetEPAInfoByEmployeeCriteria criteria);
}