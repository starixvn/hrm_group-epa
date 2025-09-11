using SMS.EPA.Core.Services;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.Report;
using SMS.EPA.Models.Report.EPARecommendation;
using SMS.EPA.Services.Interfaces;
using SMS.EPA.Services.Interfaces.ReportService;

namespace SMS.EPA.Services.Implement.ReportServices;

public class RecomendationReportService(IPeriodService periodService, IBaseService baseService)
    : IRecomendationReportService
{
    private readonly IPeriodService periodService = periodService ?? new PeriodService();
    private readonly IBaseService baseService = baseService ?? new BaseService();

    public RecomendationReportService() : this(null, null)
    {
    }

    public EPASummaryRecommendationReportModel GetRecommendationReport(int periodId)
    {
        var criteria = new GetEPARecommendationReportCriteria
        {
            PeriodId = periodId
        };

        var recommendationReport = new EPASummaryRecommendationReportModel
        {
            Recommendations = baseService.GetData<EPASummaryRecommendationModel>(criteria),
            Period = periodService.GetPeriodById(periodId)
        };

        return recommendationReport;
    }
}