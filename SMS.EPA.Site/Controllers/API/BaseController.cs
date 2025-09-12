using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Session;

namespace SMS.EPA.Site.Controllers.API;

using Microsoft.AspNetCore.Mvc;
using Core.Constants;

public class BaseController(IUserControlSession userControlSession) : Controller
{
    protected IUserControlSession userControlSession = userControlSession;

    protected int CurrentEmployeeId => userControlSession.Get<int>(Constants.CurrentEmployeeIdKey);

    protected byte GetPermissionOnFunction(string functionKey)
    {
        var policy = (byte)Policy.None;
        if (userControlSession.HasPermissionOnFunction(functionKey, Policy.Host))
        {
            policy = (byte)Policy.Host;
        }
        else
        {
            if (userControlSession.HasPermissionOnFunction(functionKey, Policy.Read))
            {
                policy = (byte)Policy.Read;
            }
        }

        return policy;
    }
}