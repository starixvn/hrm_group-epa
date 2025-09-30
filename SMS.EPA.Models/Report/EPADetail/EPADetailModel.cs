using System.Collections.Generic;
using SMS.EPA.Models.EmployeeEPA;

namespace SMS.EPA.Models.Report.EPADetail;

public class EPADetailModel : EPAReportConditionalFilename
{
    public IEnumerable<EPAModel> EpaInfos { get; set; } = new List<EPAModel>();

    public IEnumerable<EvaluationPhaseModel> PhaseSelecteds { get; set; } = new List<EvaluationPhaseModel>();
}