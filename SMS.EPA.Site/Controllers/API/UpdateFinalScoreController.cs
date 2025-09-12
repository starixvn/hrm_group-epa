using GO.UserControl.Client.Core.Session;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.EmployeeEPA;
using SMS.EPA.Models.EmployeeEPA;
using SMS.EPA.Services.Implement;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Site.Controllers.API;

using Microsoft.AspNetCore.Mvc;
using GO.UserControl.Client.Core.Enumerables;
using Core.Attribute;
using Core.Constants;
using SMS.EPA.Core.Services;

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
        var epaInfos = evaluationService.GetEPAToUpdateFinalScore(criteria);

        return Ok(epaInfos);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.UpdateFinalScore, Policy = Policy.Host)]
    public IActionResult UpdateFinalScore(UpdateFinalScoreParam param)
    {
        var commandResult = processService.UpdateTotalScoreOfProcess(param.PeriodId, param.EmployeeIds);
        if (commandResult.IsSuccessful)
        {
            commandResult = epaService.GetDataAndUpdateFinalScore(CurrentEmployeeId, param.PeriodId, param.EmployeeIds);
        }

        return Ok(commandResult);
    }
}