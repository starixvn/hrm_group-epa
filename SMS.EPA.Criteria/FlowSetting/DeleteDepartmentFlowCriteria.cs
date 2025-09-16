using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.FlowSetting;

public class DeleteDepartmentFlowCriteria : CriteriaBase
{
    public int DepartmentId { get; set; }

    public int FlowTemplateId { get; set; }

    public bool IsGroup { get; set; }

    public int ModifiedBy { get; set; }

    public override bool IsValid() => DepartmentId > 0 && FlowTemplateId > 0 && ModifiedBy > 0;

    public override string GetSettingKey() => "SettingGroupFlow_Delete";
}