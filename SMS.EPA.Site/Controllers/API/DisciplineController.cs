namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using Core.Attribute;
using Core.Constants;
using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Session;
using Services.Interfaces;
using SMS.EPA.Criteria.Discipline;
using SMS.EPA.Models.Discipline;

public class DisciplineController(IUserControlSession session, IDisciplineService disciplineService)
    : BaseController(session)
{
    [ApiAuthorize(FunctionKey = EPAFunctionKey.DisciplineSummary, Policy = Policy.Read)]
    public IEnumerable<DisciplineSummaryModel> GetDisciplineSummary(GetDisciplineSummaryCriteria criteria)
    {
        criteria.ApproverId = CurrentEmployeeId;
        var disciplines = disciplineService.GetDisciplineSummary(criteria);
        return disciplines;
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.DisciplineSummaryByPeriod, Policy = Policy.Read)]
    public IEnumerable<DisciplineSummaryForOtherSystem> GetDisciplineSummaryForOtherSystem(GetDisciplineSummaryForOtherSystemCriteria criteria)
    {
        var disciplines = disciplineService.GetDisciplineSummaryForOtherSystem(criteria);
        return disciplines;
    }
}