using System.Collections.Generic;
using SMS.EPA.Criteria.KPI;
using SMS.EPA.Models.KPI;

namespace SMS.EPA.Services.Interfaces;

public interface IKPIService
{
    IEnumerable<KPIModel> GetKPI(GetKPICriteria criteria);
}