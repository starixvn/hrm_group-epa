using System.Collections.Generic;
using Fanex.Data.Repository;
using SMS.EPA.Models.FlowTemplate;

namespace SMS.EPA.Repositories.Interfaces;

public interface IFlowTemplateRepository
{
    IEnumerable<FlowTemplateModel> GetFlowTemplate(ICriteria criteria);

    FlowTemplateModel GetFlowTemplateById(ICriteria criteria);
}