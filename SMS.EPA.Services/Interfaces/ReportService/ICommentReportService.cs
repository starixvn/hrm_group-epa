using System.Collections.Generic;
using SMS.EPA.Models.Report.EPAComment;

namespace SMS.EPA.Services.Interfaces.ReportService;

public interface ICommentReportService
{
    IEnumerable<EPASummaryCommentModel> GetSummaryComment(int periodId);
}