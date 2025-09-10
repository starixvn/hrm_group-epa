namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Session;
using SMS.EPA.Core.Attribute;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Model;
using SMS.EPA.Core.Services;
using SMS.EPA.Criteria;
using SMS.EPA.Model;
using SMS.EPA.Services.Implement;
using SMS.EPA.Services.Interfaces;

public class FlowTemplateController(
    IUserControlSession userControlSession,
    IFlowTemplateService flowTemplateService,
    IBaseService baseService)
    : BaseController(userControlSession)
{
    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Read)]
    [HttpGet]
    public IEnumerable<FlowTemplateModel> GetFlowTemplate(
        int? targetTypeId = null,
        bool? isActive = null)
    {
        var criteria = new GetFlowTemplateCriteria
        {
            TargetTypeId = targetTypeId,
            IsActive = isActive
        };

        return flowTemplateService.GetFlowTemplate(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Read)]
    [HttpGet]
    public FlowTemplateModel GetFlowTemplateById(int flowTemplateId)
    {
        return flowTemplateService.GetFlowTemplateById(flowTemplateId);
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Host)]
    public CommandResult CreateFlowTemplate(CreateFlowTemplateCriteria criteria)
    {
        criteria.CreatedBy = CurrentEmployeeId;
        return baseService.ExecuteNonQuery(criteria);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Host)]
    public CommandResult UpdateFlowTemplate(UpdateFlowTemplateCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        return baseService.ExecuteNonQuery(criteria);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Host)]
    public CommandResult UpdateStatusFlowTemplate(UpdateStatusFlowTemplateCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        return baseService.ExecuteNonQuery(criteria);
    }


    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Host)]


    public CommandResult DeleteFlowTemplate(int flowTemplateId)
    {
        var criteria = new DeleteFlowTemplateCriteria
        {
            FlowTemplateId = flowTemplateId,
            ModifiedBy = CurrentEmployeeId
        };

        return baseService.ExecuteNonQuery(criteria);
    }
}