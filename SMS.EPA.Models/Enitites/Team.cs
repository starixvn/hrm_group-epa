using System;

namespace SMS.EPA.Models.Enitites;

[Serializable]
public class Team
{
    public int TeamId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int? DepartmentId { get; set; }
}