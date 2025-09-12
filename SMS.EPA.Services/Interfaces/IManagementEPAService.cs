using SMS.EPA.Criteria.EmployeeEPA;
using SMS.EPA.Models.EmployeeEPA;

namespace SMS.EPA.Services.Interfaces;

using System.Collections.Generic;

public interface IManagementEPAService
{
    IEnumerable<EPAModel> GetEPAByManagement(GetEPAByManagementCriteria criteria);

    EPAModel GetEPAInfoByManagement(GetEPAInfoByManagementCriteria criteria);
}