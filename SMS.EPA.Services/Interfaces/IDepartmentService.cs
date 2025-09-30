using System.Collections.Generic;
using SMS.EPA.Models.Enitites;

namespace SMS.EPA.Services.Interfaces;

public interface IDepartmentService
{
    IList<MultiSelectItem> GetOrgChart();
}