using System.Threading.Tasks;
using SMS.EPA.Models.Report.EPAComment;

namespace SMS.EPA.Services.Interfaces.ExportFile;

public interface IExportCommentReportService
{
    Task Export(EPASummaryCommentReportModel data, string templatePath);
}