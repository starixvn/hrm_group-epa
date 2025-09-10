using System.Threading.Tasks;
namespace SMS.EPA.Site.Controllers;

using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Fanex.Data;
using GO.UserControl.Client.Core.Models.Auth;
using GO.UserControl.Client.Core.Services;
using GO.UserControl.Client.Core.Session;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Model;
using SMS.EPA.Core.Services;
using SMS.EPA.Criteria;
using SMS.EPA.Model.Authentication;
using Microsoft.Extensions.Configuration;

public class AuthController(
    IUserControlSession userControlSession,
    ICryptoService cryptoService,
    IAuthService authService,
    IUserService userService,
    IRoleService roleService,
    IBaseService baseService,
    IConfiguration configuration)
    : Controller
{
    public IActionResult Login()
    {
        byte.TryParse(configuration["SiteId"], out byte siteId);
        ViewBag.SiteId = siteId;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        byte.TryParse(configuration["SiteId"], out byte siteId);
        ViewBag.SiteId = siteId;

        if (!ModelState.IsValid || model == null)
        {
            return View(model);
        }

        var token = cryptoService.GenerateRijndaelKeyIV();
        string[] keyiv = token.Split(':');
        var rijndaelKey = keyiv[0];
        var rijndaelIV = keyiv[1];
        var loginParam = new LoginParam
        {
            Username = cryptoService.Encrypt(model.Username, rijndaelKey, rijndaelIV),
            Password = cryptoService.Encrypt(model.Password, rijndaelKey, rijndaelIV),
            Token = token,
            IP = GetClientIP(),
            Platform = Request.Headers["User-Agent"].ToString(),
            BrowserName = Request.Headers["User-Agent"].ToString(),
            NetSessionId = HttpContext.Session.Id
        };
        var loginResult = authService.Login(loginParam);
        if (loginResult.IsSuccessfull)
        {
            UserSession employee = await GetEmployeeLogin(loginResult.UserId);
            if (employee == null)
            {
                ModelState.AddModelError(string.Empty, "User has not been mapped.");
                return View(model);
            }

            userControlSession.UserId = loginResult.UserId;
            userControlSession.SiteId = 2; //userControlSettings.SiteId;
            userControlSession.JwtToken = loginResult.JwtToken;
            userControlSession.MenuItems = await userService.GetMenuItemAsync();
            userControlSession.Functions = await userService.GetFunctionPermissionAsync(userControlSession.SiteId, userControlSession.UserId);

            userControlSession.Set(Constants.CurrentEmployeeKey, employee);
            userControlSession.Set(Constants.CurrentEmployeeIdKey, employee.EmployeeId);

            var roles = await roleService.GetByUserIdAsync(loginResult.UserId);
            userControlSession.Set(Constants.CurrentEmployeeRoleKey, roles);

            //FormsAuthentication.SetAuthCookie(employee.EmployeeId.ToString(), false);
            Response.Redirect(Url.IsLocalUrl(model.ReturnPath) ? model.ReturnPath : "/#/");
        }
        else
        {
            ModelState.AddModelError(string.Empty, loginResult.ErrorMessage);
        }

        return Redirect("/#/");
    }

    public async Task<IActionResult> SSO(string userId, string onUserId, string time, string redirectUrl = null)
    {
        const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        var userIdValue = Cryptography.DecryptPassword(userId.Replace(" ", "+"));
        var onUserIdValue = Cryptography.DecryptPassword(onUserId.Replace(" ", "+"));
        var timeValue = Cryptography.DecryptPassword(time.Replace(" ", "+"));
        var currentTime = authService.SSOGetTime();
        TimeSpan expiration = DateTime.ParseExact(currentTime, DateTimeFormat, new CultureInfo("en-US")) - DateTime.ParseExact(timeValue, DateTimeFormat, new CultureInfo("en-US"));
        const byte TimeExpiration = 30;
        if (expiration.TotalSeconds <= TimeExpiration)
        {
            var ssoParam = new SSOLoginParam
            {
                UserId = Convert.ToInt32(userIdValue),
                CheckOnUserId = Convert.ToInt64(onUserIdValue),
                OnUserId = 0,
                IP = GetClientIP(),
                Platform = Request.Headers["User-Agent"].ToString(),
                BrowserName = Request.Headers["User-Agent"].ToString(),
                NetSessionId = HttpContext.Session.Id
            };
            LoginResult loginResult = authService.SSOLogin(ssoParam);
            if (loginResult.IsSuccessfull)
            {
                UserSession employee = await GetEmployeeLogin(Convert.ToInt32(userIdValue));
                if (employee == null)
                {
                    return RedirectToAction("Login");
                }

                userControlSession.UserId = Convert.ToInt32(userIdValue);
                userControlSession.SiteId = 2; //userControlSettings.SiteId;
                userControlSession.JwtToken = loginResult.JwtToken;
                userControlSession.OnUserId = loginResult.OnUserId;
                userControlSession.MenuItems = userService.GetMenuItem();
                userControlSession.Functions = userService.GetFunctionPermission(userControlSession.SiteId, userControlSession.UserId);

                userControlSession.Set(Constants.CurrentEmployeeKey, employee);
                userControlSession.Set(Constants.CurrentEmployeeIdKey, employee.EmployeeId);
                //FormsAuthentication.SetAuthCookie(employee.EmployeeId.ToString(), false);
                //return !string.IsNullOrWhiteSpace(redirectUrl) && Url.IsLocalUrl(redirectUrl)
                //    ? Redirect(string.Format("/#/{0}", redirectUrl)) : Redirect(FormsAuthentication.DefaultUrl);
            }
        }

        return RedirectToAction("Login");
    }


    public async Task<IActionResult> Logout()
    {
        userControlSession.ClearAll();
        //FormsAuthentication.SignOut();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    private async Task<UserSession> GetEmployeeLogin(int userId)
    {
        var criteria = new GetEmployeeLoginByUserIdCriteria
        {
            UserId = userId
        };
        return await baseService.GetDataEntityAsync<UserSession>(criteria);
    }

    private string GetClientIP()
    {
        var clientIp = Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(clientIp))
        {
            var forwardedIps = clientIp.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            clientIp = forwardedIps[forwardedIps.Length - 1];
        }
        else
        {
            clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        if (clientIp == "::1")
        {
            clientIp = "127.0.0.1";
        }

        return clientIp;
    }
}