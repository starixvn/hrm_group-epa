namespace SMS.EPA.Site.Controllers;

using Microsoft.AspNetCore.Mvc;
using GO.UserControl.Client.Core.Session;
using Core.Constants;
using Core.Model;

public class HomeController(IUserControlSession userControlSession) : Controller
{
    public ActionResult Index()
    {
        var employee = userControlSession.Get<UserSession>(Constants.CurrentEmployeeKey);
        if (employee != null)
        {
            return View(employee);
        }

        return RedirectToAction("Login", "Auth");
    }
}