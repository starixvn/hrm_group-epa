using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.TargetTemplate;

public class DeleteKPIGroupOfTargetTemplateCriteria : CriteriaBase
{
    public int TargetTemplateId { get; set; }

    public int KPIGroupId { get; set; }

    public int ModifiedBy { get; set; }

    public override bool IsValid()
    {
        return TargetTemplateId > 0 && KPIGroupId > 0 && ModifiedBy > 0;
    }

    public override string GetSettingKey()
    {
        return "TargetTemplate_RemoveKPIGroup";
    }
}