using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.FlowSetting;
using SMS.EPA.Criteria.FlowTemplate;
using SMS.EPA.Models.FlowTemplate;

namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Session;
using Core.Attribute;
using Core.Constants;
using Core.Model;
using Services.Interfaces;

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
    public CommandResult CreateFlowTemplate([FromBody] CreateFlowTemplateCriteria criteria)
    {
        criteria.CreatedBy = CurrentEmployeeId;
        return baseService.ExecuteNonQuery(criteria);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Host)]
    public CommandResult UpdateFlowTemplate([FromBody] UpdateFlowTemplateCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        return baseService.ExecuteNonQuery(criteria);
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Host)]
    public CommandResult UpdateStatusFlowTemplate([FromBody] UpdateStatusFlowTemplateCriteria criteria)
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