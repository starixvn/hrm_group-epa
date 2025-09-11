using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SMS.EPA.Models.Report.EPAComment;
using SMS.EPA.Services.Implement.ExportFile;
using SMS.EPA.Services.Implement.ReportServices;
using SMS.EPA.Services.Interfaces.ExportFile;
using SMS.EPA.Services.Interfaces.ReportService;

namespace SMS.EPA.Site.Controllers.Export;

public class ExportCommentReportController(
    ICommentReportService commentReportService,
    IExportCommentReportService exportExcelService,
    IWebHostEnvironment webHostEnvironment)
    : Controller
{
    private const string CommentReportTemplate = @"~\ExcelTemplates\CommentReportTemplate.xls";
    private readonly ICommentReportService _commentReportService = commentReportService ?? new CommentReportService();
    private readonly IExportCommentReportService _exportExcelService = exportExcelService ?? new ExportCommentReportService();

    public ExportCommentReportController()
        : this(null, null, null)
    {
    }

    public ActionResult ExportExcel(int periodId, string periodName)
    {
        var data = new EPASummaryCommentReportModel
        {
            CommentSummary = _commentReportService.GetSummaryComment(periodId),
            PeriodId = periodId,
            PeriodName = periodName
        };

        _exportExcelService.Export(data, Path.Combine(webHostEnvironment.WebRootPath, CommentReportTemplate));

        return new EmptyResult();
    }
}