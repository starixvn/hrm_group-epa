using System.Collections.Generic;
using SMS.EPA.Models.Enitites;

namespace SMS.EPA.Services.Interfaces;

public interface IUnitService
{
    IList<Unit> Get(bool? isActive = true);

    IList<MultiSelectItem> GetOrgChart();
}