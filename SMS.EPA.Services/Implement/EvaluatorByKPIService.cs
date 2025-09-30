using SMS.EPA.Criteria.EmployeeEPA.RatingByKPI;
using SMS.EPA.Models.EmployeeEPA.RatingByKPI;
using SMS.EPA.Repositories.Implement;
using SMS.EPA.Repositories.Interfaces;

namespace SMS.EPA.Services.Implement;

using System.Collections.Generic;
using Interfaces;

public class EvaluatorByKPIService(IEmployeeEPARepository evaluationRepository) : IEvaluatorByKPIService
{
    private readonly IEmployeeEPARepository evaluationRepository = evaluationRepository ?? new EmployeeEPARepository();

    public EvaluatorByKPIService() : this(null)
    {
    }

    public IEnumerable<TemporaryKPIModel> GetKPIByTargetTemplate(GetKPIByTargetTemplateCriteria criteria)
    {
        IEnumerable<TemporaryKPIModel> kpis = new List<TemporaryKPIModel>();

        if (criteria != null && criteria.IsValid())
        {
            kpis = evaluationRepository.GetKPIByTargetTemplate(criteria);
        }

        return kpis;
    }

    public IEnumerable<TemporaryKPIModel> GetKPIToView(GetKPIToViewCriteria criteria)
    {
        IEnumerable<TemporaryKPIModel> kpis = new List<TemporaryKPIModel>();

        if (criteria != null && criteria.IsValid())
        {
            kpis = evaluationRepository.GetKPIByTargetTemplate(criteria);
        }

        return kpis;
    }

    public IEnumerable<RatingByKPIModel> GetEmployeeEPAToView(GetEmployeeEPAToViewCriteria criteria)
    {
        IEnumerable<RatingByKPIModel> epas = new List<RatingByKPIModel>();

        if (criteria != null && criteria.IsValid())
        {
            epas = evaluationRepository.GetEmployeeEPAByKPI(criteria);
        }

        return epas;
    }

    public IEnumerable<RatingByKPIModel> GetEmployeeEpabyKPI(GetEmployeeEPAByKPICriteria criteria)
    {
        IEnumerable<RatingByKPIModel> epas = new List<RatingByKPIModel>();

        if (criteria != null && criteria.IsValid())
        {
            epas = evaluationRepository.GetEmployeeEPAByKPI(criteria);
        }

        return epas;
    }
}