using SMS.EPA.Models.EmployeeEPA;
using SMS.EPA.Repositories.Implement;
using SMS.EPA.Repositories.Interfaces;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Services.Implement;

using System.Collections.Generic;
using System.Linq;
using Fanex.Data.Repository;

public abstract class BaseActorEPAService(
    IEmployeeEPARepository evaluationRepository,
    IEmployeeEPAService evaluationService)
{
    protected BaseActorEPAService()
        : this(null, null)
    {
    }

    protected IEmployeeEPARepository EvaluationRepository { get; } = evaluationRepository ?? new EmployeeEPARepository();

    protected IEmployeeEPAService EvaluationService { get; } = evaluationService ?? new EmployeeEPAService();

    public IEnumerable<EPAModel> GetEPAByActor(ICriteria criteria)
    {
        IEnumerable<EPAModel> epaInfos = new List<EPAModel>();

        if (criteria != null && criteria.IsValid())
        {
            epaInfos = EvaluationRepository.GetEPA(criteria);

            IList<EPAModel> monthlyEPAs = new List<EPAModel>();
            var epaIds = string.Join(",", epaInfos.Where(x => x.IsSemiHasMonthly).Select(x => x.EPAId));

            if (!string.IsNullOrEmpty(epaIds))
            {
                monthlyEPAs = EvaluationService.GetMonthlyEPAOfSemi(epaIds);
            }

            UpdateEPAInfos(criteria, epaInfos, monthlyEPAs);
        }

        return epaInfos;
    }

    protected abstract void UpdateEPAInfos(
        ICriteria criteria,
        IEnumerable<EPAModel> epaInfos,
        IList<EPAModel> monthlyEPAs);
}