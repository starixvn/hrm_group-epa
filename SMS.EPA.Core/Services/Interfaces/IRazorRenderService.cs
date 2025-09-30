using Microsoft.AspNetCore.Mvc;

namespace SMS.EPA.Core.Services.Interfaces;

public interface IRazorRenderService
{
    string RenderViewToString(ControllerBase controller, string viewPath, object model);
}