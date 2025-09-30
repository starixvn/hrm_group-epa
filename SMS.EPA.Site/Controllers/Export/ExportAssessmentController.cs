using GO.UserControl.Client.Core.Session;
using SMS.EPA.Services.Implement.JobLevelStandard;
using SMS.EPA.Services.Interfaces.JobLevelStandard;

namespace SMS.EPA.Site.Controllers.Export;

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Core.Constants;
using Criteria.JobLevelStandard.Report;
using Resources;
using Services.Implement.ExportFile;
using Services.Interfaces.ExportFile;

public class ExportAssessmentController(
    IUserControlSession userControlSession,
    IJobLevelAssessmentService jobLevelAssessmentService,
    IExportJobLevelAssessmentService exportService)
    : Controller
{
    private readonly IJobLevelAssessmentService jobLevelAssessmentService = jobLevelAssessmentService ?? new JobLevelAssessmentService();
    private readonly IExportJobLevelAssessmentService exportService = exportService ?? new ExportJobLevelAssessmentService();

    public ExportAssessmentController()
        : this(null, null, null)
    {
    }

    public ActionResult ExportJoblevelAssessment(GetJobLevelAssessmentReportCriteria criteria)
    {
        criteria.ActorId = userControlSession.Get<int>(Constants.CurrentEmployeeIdKey);
        var assessments = jobLevelAssessmentService.GetJobLevelAssessmentReport(criteria);

        var kpis = assessments
            .SelectMany(ass => ass.KPIs)
            .GroupBy(kpi => new { kpi.KPIName, kpi.KPIId })
            .Select(kpi => kpi.First())
            .OrderBy(kpi => kpi.KPIId)
            .ToList();

        var title = Lang.JoblevelAssessmentReportTitle + " - " + criteria.PeriodName;
        var output = exportService.ExportJoblevelAssessmentReport(kpis, assessments, title);

        return File(output.ToArray(), "application/vnd.ms-excel", $"{Lang.JoblevelAssessmentReportName}.xls");
    }
}