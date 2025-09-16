using System.Threading.Tasks;
using SMS.EPA.Models.Report.EPARecommendation;

namespace SMS.EPA.Services.Interfaces.ExportFile;

public interface IExportRecommendationReportService
{
    Task Export(EPASummaryRecommendationReportModel recommendations, string templatePath);
}