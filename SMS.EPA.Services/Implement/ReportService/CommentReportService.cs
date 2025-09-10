namespace SMS.EPA.Services.Implement;

using System.Collections.Generic;
using SMS.EPA.Core.Services;
using SMS.EPA.Criteria.Report;
using SMS.EPA.Model;
using SMS.EPA.Services.Interfaces;

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