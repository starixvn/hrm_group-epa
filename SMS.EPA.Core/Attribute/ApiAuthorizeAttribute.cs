using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SMS.EPA.Core.Attribute;

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Session;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiAuthorizeAttribute : System.Attribute, IAuthorizationFilter
{
    public string FunctionKey { get; set; }

    public Policy Policy { get; set; }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor.EndpointMetadata.Any(item => item is AllowAnonymousAttribute))
        {
            return;
        }

        var session = context.HttpContext.RequestServices.GetRequiredService<IUserControlSession>();
        if (session.UserId <= 0 || session.JwtToken == null || session.JwtToken.IsExpired)
        {
            if (IsAjaxRequest(context.HttpContext.Request))
            {
                context.Result = new UnauthorizedResult();
            }
            else
            {
                context.Result = new RedirectToActionResult("logout", "auth", null);
            }
        }

        CheckUserPermission(context, session);
    }

    private static bool IsAjaxRequest(HttpRequest request)
        => string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.OrdinalIgnoreCase) ||
           string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);

    private void CheckUserPermission(AuthorizationFilterContext actionContext, IUserControlSession session)
    {
        var hasPermission = false;
        if (!string.IsNullOrWhiteSpace(FunctionKey))
        {
            var functionKeys = FunctionKey.Split(',').ToList();
            hasPermission = functionKeys.Exists(x => session.HasPermissionOnFunction(x, Policy));
        }

        if (!hasPermission)
        {
            actionContext.Result = new RedirectToActionResult("AccessDenied", "Error", null);
        }
    }
}