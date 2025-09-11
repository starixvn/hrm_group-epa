using SMS.EPA.Criteria.EmployeeEPA;
using SMS.EPA.Models.EmployeeEPA;

namespace SMS.EPA.Services.Implement;

using System.Collections.Generic;
using System.Linq;
using Criteria;
using Fanex.Data.Repository;
using Interfaces;
using SMS.EPA.Core.Enumerations;

public class EvaluatorEPAService(ICommonService commonService) : BaseActorEPAService(null, null), IEvaluatorEPAService
{
    private readonly ICommonService commonService = commonService ?? new CommonService();

    public EvaluatorEPAService() : this(null)
    {
    }

    public IEnumerable<EPAModel> GetEPAByEvaluator(GetEPAByEvaluatorCriteria criteria)
    {
        return GetEPAByActor(criteria);
    }

    protected override void UpdateEPAInfos(
        ICriteria criteria,
        IEnumerable<EPAModel> epaInfos,
        IList<EPAModel> monthlyEPAs)
    {
        GetEPAByEvaluatorCriteria getEPAByEvaluatorCriteria = criteria as GetEPAByEvaluatorCriteria;

        foreach (EPAModel epaInfo in epaInfos)
        {
            if (getEPAByEvaluatorCriteria != null)
            {
                epaInfo.Process.CurrentProcess =
                    EvaluationService.CloneEPAProcess(
                        epaInfo.Process.Evaluators.FirstOrDefault(
                            x => x.EvaluatorInfo.EmployeeId == getEPAByEvaluatorCriteria.EvaluatorId));
            }

            EvaluationService.SetMaxPerformanceScore(epaInfo);
            EvaluationService.GroupMonthlyEPABySemi(epaInfo, monthlyEPAs);
            EvaluationService.ConvertAverageMonthlyScore(epaInfo.MonthlyEPA, epaInfo.ScoreRatingType);
        }
    }

    public EPAModel GetEPAInfoByEvaluator(GetEPAInfoByEvaluatorCriteria criteria)
    {
        var epaInfo = new EPAModel();
        if (criteria != null && criteria.IsValid())
        {
            epaInfo = EvaluationRepository.GetEPAInfo(criteria);
            epaInfo.JobDescriptions = commonService.GetJobDescriptionByEPAId(criteria.EPAId);
            epaInfo.Process.CurrentProcess =
                EvaluationService.CloneEPAProcess(
                    epaInfo.Process.Evaluators.FirstOrDefault(
                        x => x.EvaluatorInfo.EmployeeId == criteria.EvaluatorId));
            EvaluationService.SetMaxPerformanceScore(epaInfo);
            EvaluationService.SpreadKPIScore(epaInfo.KPIGroups);

            foreach (EPAKpiModel kpi in epaInfo.KPIGroups.SelectMany(x => x.KPIs))
            {
                kpi.CurrentEvaluation = kpi.EvaluationByEvaluators.FirstOrDefault();
            }

            if (epaInfo.IsSemiHasMonthly)
            {
                epaInfo.MonthlyEPA.MonthlyEPAs = EvaluationService.GetMonthlyEPAOfSemi(epaInfo.EPAId.ToString());
                EvaluationService.ConvertAverageMonthlyScore(epaInfo.MonthlyEPA, epaInfo.ScoreRatingType);
            }

            if (epaInfo.IsProbation || epaInfo.IsPromotion || epaInfo.IsSemiAnnual)
            {
                epaInfo.Recommendations = EvaluationService.GetRecommendationByEPAId(epaInfo.EPAId).Where(x => x.ProcessId == (byte)ProcessType.Evaluator);
            }

            if (epaInfo.IsProbation)
            {
                epaInfo.ExtendedQuestion = EvaluationService.GetExtendedQuestionByEPAId(epaInfo.EPAId);
            }
        }

        return epaInfo;
    }
}