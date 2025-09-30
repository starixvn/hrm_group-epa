using System;

namespace SMS.EPA.Models.Enitites;

[Serializable]
public class Section
{
    public int SectionId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int? DepartmentId { get; set; }
}