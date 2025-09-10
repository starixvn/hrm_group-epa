namespace SMS.EPA.Core.Attribute;

using System;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class CompressContentAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        // Compression is handled by ASP.NET Core middleware
        // This attribute is kept for compatibility but functionality is moved to middleware
    }
}