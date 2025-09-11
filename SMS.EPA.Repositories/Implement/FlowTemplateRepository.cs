using System.Collections.Generic;
using Fanex.Data;
using Fanex.Data.Repository;
using SMS.EPA.Models.FlowTemplate;
using SMS.EPA.Models.TargetType;
using SMS.EPA.Repositories.Interfaces;

namespace SMS.EPA.Repositories.Implement;

public class FlowTemplateRepository : IFlowTemplateRepository
{
    public IEnumerable<FlowTemplateModel> GetFlowTemplate(ICriteria criteria)
    {
        using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
        {
            return db.Query<FlowTemplateModel, TargetTypeModel, FlowTemplateModel>(
                (ft, tt) =>
                {
                    ft.TargetType = tt;
                    return ft;
                },
                "TargetTypeId",
                criteria);
        }
    }

    public FlowTemplateModel GetFlowTemplateById(ICriteria criteria)
    {
        using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
        {
            return db.QueryEntity<FlowTemplateModel, TargetTypeModel, FlowTemplateModel>(
                (ft, tt) =>
                {
                    ft.TargetType = tt;
                    return ft;
                },
                "TargetTypeId",
                criteria);
        }
    }
}