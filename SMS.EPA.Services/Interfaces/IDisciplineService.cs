using SMS.EPA.Criteria.Discipline;
using SMS.EPA.Models.Discipline;

namespace SMS.EPA.Services.Interfaces;

using System.Collections.Generic;
using Criteria;

public interface IDisciplineService
{
    IEnumerable<DisciplineSummaryModel> GetDisciplineSummary(GetDisciplineSummaryCriteria criteria);

    IEnumerable<DisciplineSummaryForOtherSystem> GetDisciplineSummaryForOtherSystem(GetDisciplineSummaryForOtherSystemCriteria criteria);
}