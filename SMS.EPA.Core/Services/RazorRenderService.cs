namespace SMS.EPA.Core.Services;

using Microsoft.AspNetCore.Mvc;

public class RazorRenderService : IRazorRenderService
{
    public string RenderViewToString(ControllerBase controller, string viewPath, object model)
    {
        // This implementation needs to be updated for ASP.NET Core
        // For now, returning a placeholder
        return $"View: {viewPath}, Model: {model?.GetType().Name ?? "null"}";
    }
}