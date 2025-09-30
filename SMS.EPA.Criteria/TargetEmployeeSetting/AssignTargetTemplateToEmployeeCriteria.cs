using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.TargetEmployeeSetting;

public class AssignTargetTemplateToEmployeeCriteria : CriteriaBase
{
    public int TargetTemplateId { get; set; }

    public string EmployeeIds { get; set; }

    public int ModifiedBy { get; set; }

    public override bool IsValid() => TargetTemplateId > 0 && ModifiedBy > 0;

    public override string GetSettingKey() => "EpaTargetTemplateEmployee_Assign";
}