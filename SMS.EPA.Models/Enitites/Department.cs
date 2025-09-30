using System;

namespace SMS.EPA.Models.Enitites;

[Serializable]
public class Department
{
    public int DepartmentId { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public int UnitId { get; set; }

    public bool IsActive { get; set; }

    public bool IsGroup { get; set; }
}