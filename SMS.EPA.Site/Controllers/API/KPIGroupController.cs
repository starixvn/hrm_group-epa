using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.KPIGroup;
using SMS.EPA.Models.KPIGroup;

namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using GO.UserControl.Client.Core.Session;
using System.Web.Http;
using GO.UserControl.Client.Core.Enumerables;
using Core.Attribute;
using Core.Constants;
using Core.Model;
using SMS.EPA.Core.Services;

public class KPIGroupController(IUserControlSession userControlSession, IBaseService baseService)
    : BaseController(userControlSession)
{
    private readonly IBaseService baseService = baseService ?? new BaseService();

    [ApiAuthorize(FunctionKey = EPAFunctionKey.KPIGroup, Policy = Policy.Read)]

    public IEnumerable<KPIGroupModel> GetKPIGroup(bool? isActive = null)
    {
        var criteria = new GetKPIGroupCriteria
        {
            IsActive = isActive
        };

        return baseService.GetData<KPIGroupModel>(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.KPIGroup, Policy = Policy.Read)]

    public KPIGroupModel GetKPIGroupById(int kpiGroupId)
    {
        var criteria = new GetKPIGroupByIdCriteria
        {
            KPIGroupId = kpiGroupId
        };

        return baseService.GetDataEntity<KPIGroupModel>(criteria);
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.KPIGroup, Policy = Policy.Host)]
    public CommandResult CreateKPIGroup(CreateKPIGroupCriteria criteria)
    {
        criteria.CreatedBy = CurrentEmployeeId;
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.KPIGroup, Policy = Policy.Host)]
    public CommandResult UpdateKPIGroup(UpdateKPIGroupCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.KPIGroup, Policy = Policy.Host)]
    public CommandResult UpdateStatusKPIGroup(UpdateStatusKPIGroupCriteria criteria)
    {
        criteria.ModifiedBy = CurrentEmployeeId;
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }


    [ApiAuthorize(FunctionKey = EPAFunctionKey.KPIGroup, Policy = Policy.Host)]


    public CommandResult DeleteKPIGroup(int kpiGroupId)
    {
        var criteria = new DeleteKPIGroupCriteria
        {
            KPIGroupId = kpiGroupId,
            ModifiedBy = CurrentEmployeeId
        };
        var commandResult = baseService.ExecuteNonQuery(criteria);

        return commandResult;
    }

    public byte GetUserPermissionOnKPIGroup()
    {
        return GetPermissionOnFunction(EPAFunctionKey.KPIGroup);
    }
}