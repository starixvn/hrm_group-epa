using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.TargetEmployeeSetting;

public class GetAllTargetEmployeeSettingCriteria : CriteriaBase
{
    public string TeamIds { get; set; }

    public int? EmployeeId { get; set; }

    public int? TargetTypeId { get; set; }

    public bool ShowAll { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "EpaTargetTemplateEmployee_Get";
}