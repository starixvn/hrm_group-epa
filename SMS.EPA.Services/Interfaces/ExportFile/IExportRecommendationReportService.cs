using SMS.EPA.Models.Report.EPARecommendation;

namespace SMS.EPA.Services.Interfaces.ExportFile;

public interface IExportRecommendationReportService
{
    void Export(EPASummaryRecommendationReportModel recommendations, string templatePath);
}