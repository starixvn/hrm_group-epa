using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SMS.EPA.Services.Interfaces.ExportFile;
using SMS.EPA.Services.Interfaces.ReportService;

namespace SMS.EPA.Site.Controllers.Export;

public class ExportRecommendationReportController(
    IRecomendationReportService recomendationReportService,
    IExportRecommendationReportService exportExcelService,
    IWebHostEnvironment webHostEnvironment) : Controller
{
    private const string RecommendationReportTemplate ="ExcelTemplates/RecommendationReportTemplate.xls";

    public async Task<ActionResult> ExportExcel(int periodId)
    {
        var data = recomendationReportService.GetRecommendationReport(periodId);

        await exportExcelService.Export(data, Path.Combine(webHostEnvironment.ContentRootPath, RecommendationReportTemplate));

        return new EmptyResult();
    }
}