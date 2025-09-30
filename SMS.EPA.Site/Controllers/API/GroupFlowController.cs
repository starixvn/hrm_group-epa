using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Session;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.FlowSetting;
using SMS.EPA.Models.API;
using SMS.EPA.Models.FlowSetting;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Core.Attribute;
using Core.Constants;
using Core.Model;

public class GroupFlowController(IUserControlSession session, IDepartmentFlowService departmentFlowService, IBaseService baseService) : BaseController(session)
{
    [HttpGet]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Read)]
    public IEnumerable<DepartmentFlowModel> GetByFlowTemplateId(int flowTemplateId)
    {
        return departmentFlowService.GetByFlowTemplateId(flowTemplateId);
    }

    [HttpGet]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Read)]
    public IEnumerable<FlowModel> GetById(int groupId, int flowTemplateId)
    {
        var criteria = new GetDepartmentFlowByIdCriteria
        {
            FlowTemplateId = flowTemplateId,
            GroupId = groupId,
        };

        return departmentFlowService.GetById(criteria);
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Host)]
    public CommandResult CreateDepartmentFlow([FromBody] CreateDepartmentFlowCriteria criteria)
    {
        var commandResult = new CommandResult();
        if (criteria != null)
        {
            criteria.ModifiedBy = CurrentEmployeeId;
            commandResult = baseService.ExecuteNonQuery(criteria);
        }

        return commandResult;
    }

    [HttpDelete]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Host)]
    public CommandResult DeleteDepartmentFlow(int groupId, int flowTemplateId)
    {
        var criteria = new DeleteDepartmentFlowCriteria
        {
            FlowTemplateId = flowTemplateId,
            GroupId = groupId,
            ModifiedBy = CurrentEmployeeId
        };

        return baseService.ExecuteNonQuery(criteria);
    }

    [HttpGet]
    public IEnumerable<Group> GetDepartmentHasSetting(int flowTemplateId)
    {
        var criteria = new GetDepartmentSectionHasSettingCriteria
        {
            FlowTemplateId = flowTemplateId
        };
        return baseService.GetData<Group>(criteria);
    }
}