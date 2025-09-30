using SMS.EPA.Criteria.FlowTemplate;
using SMS.EPA.Models.FlowTemplate;
using SMS.EPA.Repositories.Implement;
using SMS.EPA.Repositories.Interfaces;

namespace SMS.EPA.Services.Implement;

using System.Collections.Generic;
using Interfaces;

public class FlowTemplateService(IFlowTemplateRepository flowTemplateRepository) : IFlowTemplateService
{
    private readonly IFlowTemplateRepository flowTemplateRepository = flowTemplateRepository ?? new FlowTemplateRepository();

    public FlowTemplateService() : this(null)
    {
    }

    public IEnumerable<FlowTemplateModel> GetFlowTemplate(GetFlowTemplateCriteria criteria)
    {
        IEnumerable<FlowTemplateModel> flowTemplates = new List<FlowTemplateModel>();

        if (criteria != null && criteria.IsValid())
        {
            flowTemplates = flowTemplateRepository.GetFlowTemplate(criteria);
        }

        return flowTemplates;
    }

    public FlowTemplateModel GetFlowTemplateById(int flowTemplateById)
    {
        var flowTemplate = new FlowTemplateModel();
        var criteria = new GetFlowTemplateByIdCriteria
        {
            FlowTemplateId = flowTemplateById
        };

        if (criteria.IsValid())
        {
            flowTemplate = flowTemplateRepository.GetFlowTemplateById(criteria);
        }

        return flowTemplate;
    }
}