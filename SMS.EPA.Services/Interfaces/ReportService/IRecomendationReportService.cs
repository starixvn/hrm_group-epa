using SMS.EPA.Models.Report.EPARecommendation;

namespace SMS.EPA.Services.Interfaces.ReportService;

public interface IRecomendationReportService
{
    EPASummaryRecommendationReportModel GetRecommendationReport(int periodId);
}