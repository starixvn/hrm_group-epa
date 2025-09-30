using System.Collections.Generic;

namespace SMS.EPA.Models.JobDescription;

public class JobDescriptionGroupSetting : JobDescriptionGroupInfo
{
    public IEnumerable<JobDescriptionGroupItem> GroupItems { get; set; } = new List<JobDescriptionGroupItem>();
}