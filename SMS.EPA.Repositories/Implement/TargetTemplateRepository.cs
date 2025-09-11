using System.Collections.Generic;
using Fanex.Data;
using Fanex.Data.Repository;
using SMS.EPA.Models.KPI;
using SMS.EPA.Models.KPIGroup;
using SMS.EPA.Repositories.Interfaces;

namespace SMS.EPA.Repositories.Implement;

public class TargetTemplateRepository : ITargetTemplateRepository
{
    public IEnumerable<KPIModel> GetKPIOfTargetTemplate(ICriteria criteria)
    {
        using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
        {
            return db.Query<KPIModel, KPIGroupModel, KPIModel>(
                (kpi, gr) =>
                {
                    kpi.KPIGroups.Add(gr);
                    return kpi;
                },
                "KPIGroupId",
                criteria);
        }
    }
}