using Fanex.Data.Repository;
using SMS.EPA.Models.Report.EPADetail;
using SMS.EPA.Models.Report.EPASummary;

namespace SMS.EPA.Repositories.Interfaces;

public interface IReportRepository
{
    EPASummaryModel GetEPASummary(ICriteria criteria);

    EPADetailModel GetEPADetail(ICriteria criteria);

    EPASummaryModel GetMonthlyEPASummary(ICriteria criteria);

    EPADetailModel GetMonthlyEPADetail(ICriteria criteria);
}