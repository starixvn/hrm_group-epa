using Fanex.Data.Repository;
using SMS.EPA.Core.Enumerations;

namespace SMS.EPA.Criteria.JobLevelStandard.Assessment;

public class EditStatusJobLevelAssessmentCommand : CriteriaBase
{
    public string AssessmentIds { get; set; }
    public JobLevelAssessmentStatus StatusId { get; set; }
    public int ModifiedBy { get; set; }

    public override bool IsValid() => !string.IsNullOrWhiteSpace(AssessmentIds) && ModifiedBy > 0;

    public override string GetSettingKey() => "JobLevelStandard_Assessment_UpdateStatus";
}