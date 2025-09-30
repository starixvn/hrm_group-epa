namespace SMS.EPA.Core.Attribute;

using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Fanex.Logging;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ExceptionHandlingAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context != null)
        {
            var ex = context.Exception;
            var logMessage = ex.Message;

            if (ex is UnauthorizedAccessException)
            {
                LogService.Info(logMessage);
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
            }
            else if (ex is OperationCanceledException)
            {
                LogService.Info(logMessage);
                context.Result = new Microsoft.AspNetCore.Mvc.StatusCodeResult(500);
            }
            else
            {
                LogService.Error(logMessage);
                context.Result = new Microsoft.AspNetCore.Mvc.StatusCodeResult(500);
            }
        }
    }
}