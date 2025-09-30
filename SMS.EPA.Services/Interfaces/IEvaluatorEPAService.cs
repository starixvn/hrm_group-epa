using SMS.EPA.Criteria.EmployeeEPA;
using SMS.EPA.Models.EmployeeEPA;

namespace SMS.EPA.Services.Interfaces;

using System.Collections.Generic;

public interface IEvaluatorEPAService
{
    IEnumerable<EPAModel> GetEPAByEvaluator(GetEPAByEvaluatorCriteria criteria);

    EPAModel GetEPAInfoByEvaluator(GetEPAInfoByEvaluatorCriteria criteria);
}