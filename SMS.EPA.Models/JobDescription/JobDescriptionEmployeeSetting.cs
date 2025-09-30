using System.Collections.Generic;

namespace SMS.EPA.Models.JobDescription;

public class JobDescriptionEmployeeSetting : JobDescriptionEmployeeInfo
{
    public IEnumerable<JobDescriptionGroupItem> GroupItems { get; set; } = new List<JobDescriptionGroupItem>();
}