using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.TargetTemplate;

public class GetTargetTemplateByIdCriteria : CriteriaBase
{
    public int TargetTemplateId { get; set; }

    public override bool IsValid()
    {
        return TargetTemplateId > 0;
    }

    public override string GetSettingKey()
    {
        return "TargetTemplate_GetById";
    }
}