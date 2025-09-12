using System.Collections.Generic;
using System.Linq;
using Fanex.Data;
using Fanex.Data.Repository;
using SMS.EPA.Models.KPI;
using SMS.EPA.Models.KPIGroup;
using SMS.EPA.Repositories.Interfaces;

namespace SMS.EPA.Repositories.Implement;

public class KPIRepository : IKPIRepository
{
    public IEnumerable<KPIModel> GetKPI(ICriteria criteria)
    {
        using var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey());
        using var multi = db.QueryMultiple(criteria);
        var kpis = multi.Read<KPIModel>();
        var kpiGroups = multi.Read<dynamic>();

        foreach (var kpi in kpis)
        {
            kpi.KPIGroups = GroupKPIGroupByKPI(kpi.KPIId, kpiGroups);
        }

        return kpis;
    }

    private IList<KPIGroupModel> GroupKPIGroupByKPI(int kpiId, IEnumerable<dynamic> kpiGroups)
    {
        return kpiGroups.Where(x => x.KPIId == kpiId)
            .Select(x => new KPIGroupModel
            {
                KPIGroupId = x.KPIGroupId,
                Name = x.Name
            }).ToList();
    }
}