using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SMS.EPA.Services.Implement.ExportFile;
using SMS.EPA.Services.Implement.ReportServices;
using SMS.EPA.Services.Interfaces.ExportFile;
using SMS.EPA.Services.Interfaces.ReportService;

namespace SMS.EPA.Site.Controllers.Export;

public class ExportRecommendationReportController(
    IRecomendationReportService recomendationReportService,
    IExportRecommendationReportService exportExcelService,
    IWebHostEnvironment webHostEnvironment)
    : Controller
{
    private const string RecommendationReportTemplate ="ExcelTemplates/RecommendationReportTemplate.xls";
    private readonly IRecomendationReportService _recomendationReportService = recomendationReportService ?? new RecomendationReportService();
    private readonly IExportRecommendationReportService _exportExcelService = exportExcelService ?? new ExportRecommendationReportService();

    public ExportRecommendationReportController()
        : this(null, null, null)
    {
    }

    public ActionResult ExportExcel(int periodId)
    {
        var data = _recomendationReportService.GetRecommendationReport(periodId);

        _exportExcelService.Export(data, Path.Combine(webHostEnvironment.WebRootPath, RecommendationReportTemplate));

        return new EmptyResult();
    }
}