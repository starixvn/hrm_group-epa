using System.Collections.Generic;
using System.Linq;

namespace SMS.EPA.Models.JobDescription;

public class JobDescriptionGroupItem
{
    public int GroupItemId { get; set; }
    public string Name { get; set; }
    public IEnumerable<JobDescriptionItem> Items { get; set; } = new List<JobDescriptionItem>();
    public bool IsSelectedAll => Items.Any() && Items.All(x => x.IsSelected);

    public short DisplayOrder
    {
        get
        {
            var item = Items.FirstOrDefault(x => x.IsSelected);
            return item != null ? item.DisplayOrder : short.MaxValue;
        }
    }
}