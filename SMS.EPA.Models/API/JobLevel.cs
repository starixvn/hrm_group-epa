using System;

namespace SMS.EPA.Models.API;

public class JobLevel
{
    public int JobLevelId { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime ModifiedDate { get; set; }
}