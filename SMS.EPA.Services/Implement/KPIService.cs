using System.Collections.Generic;
using SMS.EPA.Criteria.KPI;
using SMS.EPA.Models.KPI;
using SMS.EPA.Repositories.Implement;
using SMS.EPA.Repositories.Interfaces;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Services.Implement;

public class KPIService(IKPIRepository kpiRepository) : IKPIService
{
    private readonly IKPIRepository kpiRepository = kpiRepository ?? new KPIRepository();

    public KPIService() : this(null)
    {
    }

    public IEnumerable<KPIModel> GetKPI(GetKPICriteria criteria)
    {
        IEnumerable<KPIModel> kpis = new List<KPIModel>();

        if (criteria != null && criteria.IsValid())
        {
            kpis = kpiRepository.GetKPI(criteria);
        }

        return kpis;
    }
}