using SMS.EPA.Models.RoleManagement;

namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using System.Linq;
using Core.Attribute;
using Core.Constants;
using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Models;
using GO.UserControl.Client.Core.Services;
using GO.UserControl.Client.Core.Session;
using Microsoft.AspNetCore.Mvc;

public class RoleController(
    IUserControlSession userControlSession,
    IRoleService roleService,
    IFunctionService functionService) : BaseController(userControlSession)
{
    [ApiAuthorize(FunctionKey = EPAFunctionKey.RoleManagement, Policy = Policy.Read)]
    public object GetRoleFunctionByRoleId(short roleId)
    {
        var functions = functionService.GetBySiteId(userControlSession.SiteId);
        var permissions = functionService.GetPermissionBySiteAndRoleId(userControlSession.SiteId, roleId);
        var roleFunctions = new List<RoleFunctionModel>();
        foreach (var function in functions)
        {
            var permission = permissions.FirstOrDefault(x => x.FunctionId == function.FunctionId);
            roleFunctions.Add(new RoleFunctionModel
            {
                FunctionId = function.FunctionId,
                FunctionName = function.FunctionName,
                FunctionKey = function.FunctionKey,
                ParentId = function.ParentId,
                Policy = function.Policy,
                Level = function.Level,
                DisplayOrder = function.DisplayOrder,
                Childs = new List<RoleFunctionModel>(),
                IsRead = permission != null && permission.IsRead,
                IsHost = permission != null && permission.IsHost
            });
        }

        return GetHierarchyFunction(roleFunctions);
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.RoleManagement, Policy = Policy.Host)]
    public CommandResult AssignFunctionToRole(AssignFunctionToRole model)
    {
        var commandResult = roleService.AssignPermission(userControlSession.SiteId, model.RoleId, model.Permissions);
        return commandResult;
    }

    private IList<RoleFunctionModel> GetHierarchyFunction(IList<RoleFunctionModel> functions)
    {
        IList<RoleFunctionModel> functionTrees = functions.Where(func => !func.ParentId.HasValue)
            .OrderBy(x => x.DisplayOrder).ToList();
        foreach (var function in functionTrees)
        {
            function.Level = 0;
            GetChildFunctions(function, functions);
        }

        return functionTrees;
    }

    private void GetChildFunctions(RoleFunctionModel function, IList<RoleFunctionModel> functions)
    {
        IList<RoleFunctionModel> functionItems = functions.Where(x => x.ParentId == function.FunctionId)
            .OrderBy(x => x.DisplayOrder).ToList();
        if (functionItems.Any())
        {
            function.Childs ??= new List<RoleFunctionModel>();
            foreach (var item in functionItems)
            {
                item.Level = (byte)(function.Level + 1);
                function.Childs.Add(item);
                GetChildFunctions(item, functions);
            }
        }
    }
}