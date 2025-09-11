using System.Collections.Generic;
using SMS.EPA.Core.Constants;
using SMS.EPA.Models.Period;

namespace SMS.EPA.Models.Report.EPASummary;

public class EPASummaryModel : EPAReportConditionalFilename
{
    public IList<PeriodModel> PeriodSelecteds { get; set; } = new List<PeriodModel>();

    public IList<EPASummaryEmployeeModel> EPAInfos { get; set; } = new List<EPASummaryEmployeeModel>();

    public int MaxFinalScore { get; } = Constants.MaxFinalScore;
}