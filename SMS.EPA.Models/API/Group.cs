using System;

namespace SMS.EPA.Models.API;

public class Group
{
    public int GroupId { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public int? ParentId { get; set; }

    public int ManagerId { get; set; }

    public int DepartmentLevel { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime ModifiedDate { get; set; }
}