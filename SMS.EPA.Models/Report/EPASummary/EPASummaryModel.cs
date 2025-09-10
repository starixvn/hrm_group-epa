namespace SMS.EPA.Model;

using System.Collections.Generic;
using Core.Constants;
using Report;

public class EPASummaryModel : EPAReportConditionalFilename
{
    public IList<PeriodModel> PeriodSelecteds { get; set; } = new List<PeriodModel>();

    public IList<EPASummaryEmployeeModel> EPAInfos { get; set; } = new List<EPASummaryEmployeeModel>();

    public int MaxFinalScore { get; } = Constants.MaxFinalScore;
}