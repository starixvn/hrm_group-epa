using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Session;
using SMS.EPA.Core.Attribute;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.JobLevelStandard.Report;
using SMS.EPA.Models.Enitites;
using SMS.EPA.Models.JobLevelStandard.Assessment;
using SMS.EPA.Models.JobLevelStandard.Period;
using SMS.EPA.Services.Interfaces.JobLevelStandard;

namespace SMS.EPA.Site.Controllers.API.JobLevelStandard;

public class JobLevelAssessmentReportController(
    IUserControlSession userControlSession,
    IBaseService baseService,
    IJobLevelAssessmentService jobLevelAssessmentService)
    : BaseController(userControlSession)
{
    [HttpGet]
    public IList<JobLevelStandardPeriod> GetPeriods()
    {
        var criteria = new GetPeriodCriteria();
        var periods = baseService.GetData<JobLevelStandardPeriod>(criteria);
        return periods.ToList();
    }

    [HttpGet]
    public IList<MultiSelectItem> GetOrgChart(int? periodId = null)
    {
        var criteria = new GetOrgChartByActorIdCriteria
        {
            ActorId = CurrentEmployeeId,
            PeriodId = periodId
        };
        (var departments, var sections, _) = baseService.FetchMultiple<Department, Section, Team>(criteria);

        var items = departments.Any()
            ? departments.Select(dept => new MultiSelectItem
            {
                Id = dept.DepartmentId,
                Text = dept.Name,
                Items = sections.Where(s => s.DepartmentId.HasValue && s.DepartmentId.Value == dept.DepartmentId).Select(s => new MultiSelectItem
                {
                    Id = s.SectionId,
                    Text = s.Name,
                    ParentId = dept.DepartmentId,
                    IsLeaf = true
                }).ToList()
            }).ToList()
            : sections.Select(s => new MultiSelectItem
            {
                Id = s.SectionId,
                Text = s.Name,
                IsLeaf = true
            }).ToList();

        return items;
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelAssessmentReport, Policy = Policy.Read)]
    public object GetAssessments(GetJobLevelAssessmentReportCriteria criteria)
    {
        criteria.ActorId = CurrentEmployeeId;
        var assessments = jobLevelAssessmentService.GetJobLevelAssessmentReport(criteria);

        IList<JobLevelAssessmentKPI> kpis = assessments
            .SelectMany(ass => ass.KPIs)
            .GroupBy(kpi => new { kpi.KPIName, kpi.KPIId })
            .Select(kpi => kpi.First())
            .OrderBy(kpi => kpi.KPIId)
            .ToList();

        return new { kpis, assessments };
    }
}