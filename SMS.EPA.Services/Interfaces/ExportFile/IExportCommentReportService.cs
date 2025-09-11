using SMS.EPA.Models.Report.EPAComment;

namespace SMS.EPA.Services.Interfaces.ExportFile;

public interface IExportCommentReportService
{
    void Export(EPASummaryCommentReportModel data, string templatePath);
}