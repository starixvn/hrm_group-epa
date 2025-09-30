using SMS.EPA.Criteria.FlowTemplate;
using SMS.EPA.Models.FlowTemplate;

namespace SMS.EPA.Services.Interfaces;

using System.Collections.Generic;

public interface IFlowTemplateService
{
    IEnumerable<FlowTemplateModel> GetFlowTemplate(GetFlowTemplateCriteria criteria);

    FlowTemplateModel GetFlowTemplateById(int flowTemplateById);
}