using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.FlowSetting;

public class GetDepartmentFlowByIdCriteria : CriteriaBase
{
    public int GroupId { get; set; }

    public int FlowTemplateId { get; set; }

    public override bool IsValid() => GroupId > 0 && FlowTemplateId > 0;

    public override string GetSettingKey() => "SettingGroupFlow_GetById";
}