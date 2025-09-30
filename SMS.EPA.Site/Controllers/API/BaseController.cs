namespace SMS.EPA.Site.Controllers.API;

using Core.Constants;
using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Session;
using Microsoft.AspNetCore.Mvc;

public class BaseController(IUserControlSession session) : Controller
{
    protected IUserControlSession session = session;

    protected int CurrentEmployeeId => session.Get<int>(Constants.CurrentEmployeeIdKey);

    protected byte GetPermissionOnFunction(string functionKey)
    {
        var policy = (byte)Policy.None;
        if (session.HasPermissionOnFunction(functionKey, Policy.Host))
        {
            policy = (byte)Policy.Host;
        }
        else
        {
            if (session.HasPermissionOnFunction(functionKey, Policy.Read))
            {
                policy = (byte)Policy.Read;
            }
        }

        return policy;
    }
}