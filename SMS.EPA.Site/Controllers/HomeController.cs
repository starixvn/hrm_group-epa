namespace SMS.EPA.Site.Controllers;

using Microsoft.AspNetCore.Mvc;
using GO.UserControl.Client.Core.Session;
using Core.Constants;
using Core.Model;
using Microsoft.Extensions.Configuration;

public class HomeController(IUserControlSession userControlSession, IConfiguration configuration)
    : Controller
{
    private readonly IUserControlSession userControlSession = userControlSession ?? new UserControlSession(null);

    public ActionResult Index()
    {
        byte.TryParse(configuration["SiteId"], out var siteId);
        ViewBag.SiteId = siteId;
        var employee = userControlSession.Get<UserSession>(Constants.CurrentEmployeeKey);

        if (employee != null)
        {
            return View(employee);
        }

        return RedirectToAction("Login", "Auth");
    }
}