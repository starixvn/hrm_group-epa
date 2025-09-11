using System.Collections.Generic;
using System.Linq;
using SMS.EPA.Criteria.EmployeeEPA;
using SMS.EPA.Models.EmployeeEPA;
using SMS.EPA.Repositories.Implement;
using SMS.EPA.Repositories.Interfaces;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Services.Implement;

public class SelfEvaluationService(
    IEmployeeEPARepository employeeEpaRepository,
    IEmployeeEPAService evaluationService)
    : ISelfEvaluationService
{
    private readonly IEmployeeEPARepository employeeEPARepository = employeeEpaRepository ?? new EmployeeEPARepository();
    private readonly IEmployeeEPAService evaluationService = evaluationService ?? new EmployeeEPAService();

    public SelfEvaluationService() : this(null, null)
    {
    }

    public IEnumerable<EPAModel> GetEPAByEmployee(GetEPAByEmployeeCriteria criteria)
    {
        IEnumerable<EPAModel> epas = new List<EPAModel>();
        if (criteria != null && criteria.IsValid())
        {
            epas = employeeEPARepository.GetEPA(criteria);
            var processes = epas.Select(x => x.Process);
            foreach (var process in processes)
            {
                process.CurrentProcess = evaluationService.CloneEPAProcess(process.Self);
            }
        }

        return epas;
    }

    public EPAModel GetEPAInfoByEmployee(GetEPAInfoByEmployeeCriteria criteria)
    {
        var epaInfo = new EPAModel();

        if (criteria != null && criteria.IsValid())
        {
            epaInfo = employeeEPARepository.GetEPAInfo(criteria);
            epaInfo.Process.CurrentProcess = evaluationService.CloneEPAProcess(epaInfo.Process.Self);
            evaluationService.SetMaxPerformanceScore(epaInfo);
            evaluationService.SpreadKPIScore(epaInfo.KPIGroups);

            foreach (EPAKpiModel kpi in epaInfo.KPIGroups.SelectMany(x => x.KPIs))
            {
                kpi.CurrentEvaluation = kpi.EvaluationBySelf;
            }

            if (epaInfo.IsSemiHasMonthly)
            {
                epaInfo.MonthlyEPA.MonthlyEPAs = evaluationService.GetMonthlyEPAOfSemi(epaInfo.EPAId.ToString());
                evaluationService.ConvertAverageMonthlyScore(epaInfo.MonthlyEPA, epaInfo.ScoreRatingType);
            }
        }

        return epaInfo;
    }
}