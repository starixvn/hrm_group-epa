namespace SMS.EPA.Core.Attribute;

using System;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class WebApiValidateAntiForgeryTokenAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        // Anti-forgery token validation is handled by ASP.NET Core middleware
        // This attribute is kept for compatibility but functionality is moved to middleware
        base.OnActionExecuting(context);
    }
}