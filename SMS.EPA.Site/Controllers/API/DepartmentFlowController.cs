using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Session;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.FlowSetting;
using SMS.EPA.Models.API;
using SMS.EPA.Models.FlowSetting;
using SMS.EPA.Services.Implement;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Site.Controllers.API;

using System;
using System.Collections.Generic;
using System.Web.Http;
using Core.Attribute;
using Core.Constants;
using Core.Model;
using SMS.EPA.Core.Services;

public class DepartmentFlowController(IUserControlSession session, IDepartmentFlowService departmentFlowService, IBaseService baseService) : BaseController(session)
{
    private readonly IDepartmentFlowService departmentFlowService = departmentFlowService ?? new DepartmentFlowService();
    private readonly IBaseService baseService = baseService ?? new BaseService();

    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Read)]


    public IEnumerable<DepartmentFlowModel> GetByFlowTemplateId(int flowTemplateId)
    {
        return departmentFlowService.GetByFlowTemplateId(flowTemplateId);
    }


    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Read)]


    public IEnumerable<FlowModel> GetById(int departmentId, int flowTemplateId)
    {
        var criteria = new GetDepartmentFlowByIdCriteria
        {
            FlowTemplateId = flowTemplateId,
            DepartmentId = Math.Abs(departmentId),
            IsGroup = departmentId <= 0
        };

        return departmentFlowService.GetById(criteria);
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Host)]
    public CommandResult CreateDepartmentFlow(CreateDepartmentFlowCriteria criteria)
    {
        var commandResult = new CommandResult();
        if (criteria != null)
        {
            criteria.DepartmentId = Math.Abs(criteria.DepartmentId);
            criteria.ModifiedBy = CurrentEmployeeId;
            commandResult = baseService.ExecuteNonQuery(criteria);
        }

        return commandResult;
    }


    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Host)]


    public CommandResult DeleteDepartmentFlow(int departmentId, int flowTemplateId)
    {
        var criteria = new DeleteDepartmentFlowCriteria
        {
            FlowTemplateId = flowTemplateId,
            DepartmentId = Math.Abs(departmentId),
            IsGroup = departmentId <= 0,
            ModifiedBy = CurrentEmployeeId
        };

        return baseService.ExecuteNonQuery(criteria);
    }




    public IEnumerable<Department> GetDepartmentHasSetting(int flowTemplateId)
    {
        var criteria = new GetDepartmentSectionHasSettingCriteria
        {
            FlowTemplateId = flowTemplateId
        };
        return baseService.GetData<Department>(criteria);
    }
}