using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.FlowTemplate;

public class GetFlowTemplateByIdCriteria : CriteriaBase
{
    public int FlowTemplateId { get; set; }

    public override string GetSettingKey()
    {
        return "FlowTemplate_GetById";
    }

    public override bool IsValid()
    {
        return FlowTemplateId > 0;
    }
}