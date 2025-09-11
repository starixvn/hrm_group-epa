using System.Collections.Generic;
using SMS.EPA.Core.Services;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.Report;
using SMS.EPA.Models.Report.EPAComment;
using SMS.EPA.Services.Interfaces.ReportService;

namespace SMS.EPA.Services.Implement.ReportServices;

public class CommentReportService(IBaseService baseService) : ICommentReportService
{
    private readonly IBaseService baseService = baseService ?? new BaseService();

    public CommentReportService() : this(null)
    {
    }

    public IEnumerable<EPASummaryCommentModel> GetSummaryComment(int periodId)
    {
        var criteria = new GetEPACommentCriteria
        {
            PeriodId = periodId
        };

        return baseService.GetData<EPASummaryCommentModel>(criteria);
    }
}