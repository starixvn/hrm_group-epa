namespace SMS.EPA.Models.TargetTemplate;

public class TargetTemplateDepartment
{
    public int TargetTemplateId { get; set; }

    public string Name { get; set; }

    public int DepartmentId { get; set; }

    public bool IsGroup { get; set; }

    public byte TargetTypeId { get; set; }

    public string TargetTypeName { get; set; }
}