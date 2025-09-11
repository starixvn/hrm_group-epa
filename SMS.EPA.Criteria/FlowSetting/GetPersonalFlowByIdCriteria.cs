using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.FlowSetting;

public class GetPersonalFlowByIdCriteria : CriteriaBase
{
    public int EmployeeId { get; set; }

    public int FlowTemplateId { get; set; }

    public override bool IsValid() => EmployeeId > 0 && FlowTemplateId > 0;

    public override string GetSettingKey() => "SettingEmployeeFlow_GetById";
}