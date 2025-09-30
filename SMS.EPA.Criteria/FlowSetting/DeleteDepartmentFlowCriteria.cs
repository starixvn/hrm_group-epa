using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.FlowSetting;

public class DeleteDepartmentFlowCriteria : CriteriaBase
{
    public int GroupId { get; set; }

    public int FlowTemplateId { get; set; }

    public int ModifiedBy { get; set; }

    public override bool IsValid() => GroupId > 0 && FlowTemplateId > 0 && ModifiedBy > 0;

    public override string GetSettingKey() => "SettingGroupFlow_Delete";
}