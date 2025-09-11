using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Session;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.EmployeeEPA.RatingByKPI;
using SMS.EPA.Models.EmployeeEPA;
using SMS.EPA.Models.EmployeeEPA.RatingByKPI;
using SMS.EPA.Models.Enitites;
using SMS.EPA.Models.TargetTemplate;

namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using System.Web.Http;
using SMS.EPA.Core.Attribute;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Model;
using SMS.EPA.Core.Services;
using SMS.EPA.Criteria;
using SMS.EPA.Services.Implement;
using SMS.EPA.Services.Interfaces;

public class EvaluatorByKPIController(
    IUserControlSession userControlSession,
    IEvaluatorByKPIService evaluatorByKpiService,
    IBaseService baseService)
    : BaseController(userControlSession)
{
    private readonly IEvaluatorByKPIService evaluatorByKPIService = evaluatorByKpiService ?? new EvaluatorByKPIService();
    private readonly IBaseService baseService = baseService ?? new BaseService();

    public EvaluatorByKPIController() : this(null, null, null)
    {
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.RatingByKPI, Policy = Policy.Read)]

    public IEnumerable<EPAPeriodModel> GetPeriodByEvaluator()
    {
        var criteria = new GetPeriodByEvaluatorCriteria
        {
            EvaluatorId = CurrentEmployeeId
        };

        return baseService.GetData<EPAPeriodModel>(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.RatingByKPI, Policy = Policy.Read)]

    public IEnumerable<TargetTemplate> GetTargetTemplateByEvaluator(int periodId)
    {
        var criteria = new GetTargetTemplateByEvaluatorCriteria
        {
            EvaluatorId = CurrentEmployeeId,
            PeriodId = periodId
        };

        return baseService.GetData<TargetTemplate>(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.RatingByKPI, Policy = Policy.Read)]

    public IEnumerable<EPAPeriodModel> GetPeriodToView()
    {
        var criteria = new GetPeriodToViewCriteria
        {
            EvaluatorId = CurrentEmployeeId
        };

        return baseService.GetData<EPAPeriodModel>(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.EPAByKPI, Policy = Policy.Read)]

    public IEnumerable<TargetTemplate> GetTargetTemplateToView(int periodId)
    {
        var criteria = new GetTargetTemplateToViewCriteria
        {
            EvaluatorId = CurrentEmployeeId,
            PeriodId = periodId
        };

        return baseService.GetData<TargetTemplate>(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.EPAByKPI, Policy = Policy.Read)]

    public IEnumerable<Employee> GetEvaluatorsByTargetTemplate(
        int periodId,
        int targetTemplateId)
    {
        var criteria = new GetEvaluatorsByTargetTemplateCriteria
        {
            PeriodId = periodId,
            TargetTemplateId = targetTemplateId
        };

        return baseService.GetData<Employee>(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.EPAByKPI, Policy = Policy.Read)]

    public IEnumerable<TemporaryKPIModel> GetKPIToView(
        int periodId,
        int targetTemplateId,
        int evaluatorId)
    {
        var criteria = new GetKPIToViewCriteria
        {
            EvaluatorId = evaluatorId,
            PeriodId = periodId,
            TargetTemplateId = targetTemplateId
        };

        return evaluatorByKPIService.GetKPIToView(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.EPAByKPI, Policy = Policy.Read)]

    public IEnumerable<RatingByKPIModel> GetEmployeeEPAToView(
        int periodId,
        int targetTemplateId,
        int kpiId,
        int evaluatorId)
    {
        var criteria = new GetEmployeeEPAToViewCriteria
        {
            EvaluatorId = evaluatorId,
            PeriodId = periodId,
            TargetTemplateId = targetTemplateId,
            KPIId = kpiId
        };

        return evaluatorByKPIService.GetEmployeeEPAToView(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.RatingByKPI, Policy = Policy.Read)]

    public IEnumerable<TemporaryKPIModel> GetKPIByTargetTemplate(
        int periodId,
        int targetTemplateId)
    {
        var criteria = new GetKPIByTargetTemplateCriteria
        {
            EvaluatorId = CurrentEmployeeId,
            TargetTemplateId = targetTemplateId,
            PeriodId = periodId
        };

        return evaluatorByKPIService.GetKPIByTargetTemplate(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.RatingByKPI, Policy = Policy.Read)]

    public IEnumerable<RatingByKPIModel> GetEmployeeEPAByKPI(
        int periodId,
        int targetTemplateId,
        int kpiId,
        int? groupId = null,
        string employeeIds = null)
    {
        var criteria = new GetEmployeeEPAByKPICriteria
        {
            EvaluatorId = CurrentEmployeeId,
            PeriodId = periodId,
            TargetTemplateId = targetTemplateId,
            KPIId = kpiId,
            GroupId = groupId,
            EmployeeIds = employeeIds
        };

        return evaluatorByKPIService.GetEmployeeEpabyKPI(criteria);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.RatingByKPI, Policy = Policy.Host)]
    public CommandResult UpdateKPIScore(IEnumerable<RatingByKPIModel> epaInfos)
    {
        var criteria = new UpdateKPIScoreCriteria
        {
            EvaluatorId = CurrentEmployeeId,
            EpaInfos = epaInfos
        };

        return baseService.ExecuteNonQuery(criteria);
    }

    public byte GetUserPermissionOnRatingByKPI()
    {
        return GetPermissionOnFunction(EPAFunctionKey.RatingByKPI);
    }
}