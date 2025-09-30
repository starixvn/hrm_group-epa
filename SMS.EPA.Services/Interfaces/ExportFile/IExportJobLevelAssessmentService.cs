using SMS.EPA.Models.JobLevelStandard.Assessment;

namespace SMS.EPA.Services.Interfaces.ExportFile;

using System.Collections.Generic;
using System.IO;

public interface IExportJobLevelAssessmentService
{
    MemoryStream ExportJoblevelAssessmentReport(List<JobLevelAssessmentKPI> kpis, IList<JobLevelAssessment> assessments, string title);
}