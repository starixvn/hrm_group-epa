namespace SMS.EPA.Criteria.FlowSetting;

public class CreateDepartmentFlowCriteria : CreateFlowCriteriaBase
{
    public int GroupId { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "SettingGroupFlow_Set";
}