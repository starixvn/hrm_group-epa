using System.Collections.Generic;
using SMS.EPA.Criteria.JobLevelStandard.Assessment;
using SMS.EPA.Criteria.JobLevelStandard.Report;
using SMS.EPA.Models.JobLevelStandard.Assessment;
using SMS.EPA.Models.JobLevelStandard.JobLevelSetting;

namespace SMS.EPA.Services.Interfaces.JobLevelStandard;

public interface IJobLevelAssessmentService
{
    IList<JobLevelSetting> GetJobLevelSettingByPeriod(int periodId);

    IList<JobLevelAssessment> GetJobLevelAssessmentByActor(GetJobLevelAssessmentByActorIdCriteria criteria);

    IList<JobLevelAssessment> GetJobLevelAssessmentReport(GetJobLevelAssessmentReportCriteria criteria);
}