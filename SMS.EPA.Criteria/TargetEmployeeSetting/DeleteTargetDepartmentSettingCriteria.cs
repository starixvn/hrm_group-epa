using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.TargetEmployeeSetting;

public class DeleteTargetDepartmentSettingCriteria : CriteriaBase
{
    public int TargetTemplateId { get; set; }

    public int DepartmentId { get; set; }

    public bool IsGroup { get; set; }

    public int ModifiedBy { get; set; }

    public override bool IsValid() => TargetTemplateId > 0 && DepartmentId > 0 && ModifiedBy > 0;

    public override string GetSettingKey() => "EpaTargetTemplateDepartment_Delete";
}