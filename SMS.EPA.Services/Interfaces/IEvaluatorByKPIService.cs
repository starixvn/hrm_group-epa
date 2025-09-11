using SMS.EPA.Criteria.EmployeeEPA.RatingByKPI;
using SMS.EPA.Models.EmployeeEPA.RatingByKPI;

namespace SMS.EPA.Services.Interfaces;

using System.Collections.Generic;
using Criteria;

public interface IEvaluatorByKPIService
{
    IEnumerable<TemporaryKPIModel> GetKPIByTargetTemplate(GetKPIByTargetTemplateCriteria criteria);

    IEnumerable<TemporaryKPIModel> GetKPIToView(GetKPIToViewCriteria criteria);

    IEnumerable<RatingByKPIModel> GetEmployeeEpabyKPI(GetEmployeeEPAByKPICriteria criteria);

    IEnumerable<RatingByKPIModel> GetEmployeeEPAToView(GetEmployeeEPAToViewCriteria criteria);
}