namespace SMS.EPA.Model;

using System.Collections.Generic;
using Report;

public class EPADetailModel : EPAReportConditionalFilename
{
    public IEnumerable<EPAModel> EpaInfos { get; set; } = new List<EPAModel>();

    public IEnumerable<EvaluationPhaseModel> PhaseSelecteds { get; set; } = new List<EvaluationPhaseModel>();
}