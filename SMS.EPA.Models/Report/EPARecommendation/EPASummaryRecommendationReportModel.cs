using System.Collections.Generic;
using SMS.EPA.Models.Period;

namespace SMS.EPA.Models.Report.EPARecommendation;

public class EPASummaryRecommendationReportModel
{
    public PeriodModel Period { get; set; }
    public IEnumerable<EPASummaryRecommendationModel> Recommendations { get; set; }
}