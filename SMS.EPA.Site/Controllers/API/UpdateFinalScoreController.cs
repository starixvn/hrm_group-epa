using GO.UserControl.Client.Core.Session;

namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using GO.UserControl.Client.Core.Enumerables;
using SMS.EPA.Core.Attribute;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Model;
using SMS.EPA.Core.Services;
using SMS.EPA.Criteria;
using SMS.EPA.Model;
using SMS.EPA.Model.EmployeeEPA;
using SMS.EPA.Services;

public class UpdateFinalScoreController : BaseController
{
    private readonly IEmployeeEPAService evaluationService;
    private readonly IBaseService baseService;
    private readonly IEPAProcessService processService;
    private readonly IEmployeeEPAService epaService;

    public UpdateFinalScoreController(
        IUserControlSession userControlSession,
        IEmployeeEPAService evaluationService,
        ICryptographyService cryptographyService,
        IBaseService baseService,
        IEPAProcessService processService,
        IEmployeeEPAService epaService)
        : base(userControlSession)
    {
        this.evaluationService = evaluationService ?? new EmployeeEPAService();
        this.baseService = baseService ?? new BaseService();
        this.processService = processService ?? new EPAProcessService();
        this.epaService = epaService ?? new EmployeeEPAService();
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.UpdateFinalScore, Policy = Policy.Read)]

    public IActionResult GetPeriodToUpdateFinalScore()
    {
        var criteria = new GetPeriodToUpdateFinalScoreCriteria();
        var periods = baseService.GetData<EPAPeriodModel>(criteria);

        return Ok(periods);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.UpdateFinalScore, Policy = Policy.Read)]

    public IActionResult GetEmployeeToUpdateFinalScore(
        int periodId,
        string teamIds = null,
        bool? isLocal = null,
        string query = "")
    {
        var criteria = new GetEmployeeToUpdateFinalScoreCriteria
        {
            PeriodId = periodId,
            TeamIds = teamIds,
            IsLocal = isLocal,
            Query = query
        };
        var employees = baseService.GetData<EPAEmployeeInfo>(criteria);

        return Ok(employees);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.UpdateFinalScore, Policy = Policy.Read)]

    public IActionResult GetEPAToUpdateFinalScore(
        int periodId,
        string teamIds = null,
        bool? isLocal = null,
        int? employeeId = null)
    {
        var criteria = new GetEPAToUpdateFinalScoreCriteria
        {
            PeriodId = periodId,
            IsLocal = isLocal,
            TeamIds = teamIds,
            EmployeeId = employeeId
        };
        IList<EPAModel> epaInfos = evaluationService.GetEPAToUpdateFinalScore(criteria);

        return Ok(epaInfos);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.UpdateFinalScore, Policy = Policy.Host)]
    public IActionResult UpdateFinalScore(UpdateFinalScoreParam param)
    {
        CommandResult commandResult = processService.UpdateTotalScoreOfProcess(param.PeriodId, param.EmployeeIds);
        if (commandResult.IsSuccessful)
        {
            commandResult = epaService.GetDataAndUpdateFinalScore(CurrentEmployeeId, param.PeriodId, param.EmployeeIds);
        }

        return Ok(commandResult);
    }
}