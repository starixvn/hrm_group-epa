using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.FlowTemplate;

public class DeleteFlowTemplateCriteria : CriteriaBase
{
    public int FlowTemplateId { get; set; }

    public int ModifiedBy { get; set; }

    public override string GetSettingKey()
    {
        return "FlowTemplate_Delete";
    }

    public override bool IsValid()
    {
        return FlowTemplateId > 0 && ModifiedBy > 0;
    }
}