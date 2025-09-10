namespace SMS.EPA.Core.Services;

using Microsoft.AspNetCore.Mvc;

public interface IRazorRenderService
{
    string RenderViewToString(ControllerBase controller, string viewPath, object model);
}