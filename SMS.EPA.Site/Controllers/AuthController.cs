namespace SMS.EPA.Site.Controllers;

using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Core.Constants;
using Core.Model;
using Fanex.Data;
using GO.UserControl.Client.Core;
using GO.UserControl.Client.Core.Models.Auth;
using GO.UserControl.Client.Core.Services;
using GO.UserControl.Client.Core.Session;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.Authentication;
using SMS.EPA.Models.Authentication;

public class AuthController(
    IUserControlSession userControlSession,
    ICryptoService cryptoService,
    IAuthService authService,
    IUserService userService,
    IRoleService roleService,
    IBaseService baseService) : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid || model == null)
        {
            return View(model);
        }

        var token = cryptoService.GenerateRijndaelKeyIV();
        var keyIv = token.Split(':');
        var rijndaelKey = keyIv[0];
        var rijndaelIV = keyIv[1];
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
        var loginResult = await authService.LoginAsync(loginParam);
        if (loginResult.IsSuccessfull)
        {
            userControlSession.UserId = loginResult.UserId;
            userControlSession.SiteId = UserControlClient.SiteId;
            userControlSession.JwtToken = loginResult.JwtToken;
            userControlSession.MenuItems = await userService.GetMenuItemAsync();
            userControlSession.Functions = await userService.GetFunctionPermissionAsync(userControlSession.SiteId, userControlSession.UserId);

            var employee = await GetEmployeeLogin(loginResult.UserId);
            if (employee == null)
            {
                ModelState.AddModelError(string.Empty, "User has not been mapped.");
                return View(model);
            }

            userControlSession.Set(Constants.CurrentEmployeeKey, employee);
            userControlSession.Set(Constants.CurrentEmployeeIdKey, employee.EmployeeId);

            var roles = await roleService.GetByUserIdAsync(loginResult.UserId);
            userControlSession.Set(Constants.CurrentEmployeeRoleKey, roles);

            Response.Redirect(Url.IsLocalUrl(model.ReturnPath) ? model.ReturnPath : "/");
        }
        else
        {
            ModelState.AddModelError(string.Empty, loginResult.ErrorMessage);
        }

        return Redirect("/");
    }

    public async Task<IActionResult> SSO(string userId, string onUserId, string time, string redirectUrl = null)
    {
        const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        var userIdValue = Cryptography.DecryptPassword(userId.Replace(" ", "+"));
        var onUserIdValue = Cryptography.DecryptPassword(onUserId.Replace(" ", "+"));
        var timeValue = Cryptography.DecryptPassword(time.Replace(" ", "+"));
        var currentTime = await authService.SSOGetTimeAsync();
        var expiration = DateTime.ParseExact(currentTime, DateTimeFormat, new CultureInfo("en-US")) - DateTime.ParseExact(timeValue, DateTimeFormat, new CultureInfo("en-US"));
        const byte TimeExpiration = 30;
        if (expiration.TotalSeconds > TimeExpiration)
        {
            return RedirectToAction("Login");
        }
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
        var loginResult = await authService.SSOLoginAsync(ssoParam);
        if (loginResult.IsSuccessfull)
        {
            var employee = await GetEmployeeLogin(Convert.ToInt32(userIdValue));
            if (employee == null)
            {
                return RedirectToAction("Login");
            }

            userControlSession.UserId = Convert.ToInt32(userIdValue);
            userControlSession.SiteId = UserControlClient.SiteId;
            userControlSession.JwtToken = loginResult.JwtToken;
            userControlSession.OnUserId = loginResult.OnUserId;
            userControlSession.MenuItems = await userService.GetMenuItemAsync();
            userControlSession.Functions = await userService.GetFunctionPermissionAsync(userControlSession.SiteId, userControlSession.UserId);

            userControlSession.Set(Constants.CurrentEmployeeKey, employee);
            userControlSession.Set(Constants.CurrentEmployeeIdKey, employee.EmployeeId);
            return !string.IsNullOrWhiteSpace(redirectUrl) && Url.IsLocalUrl(redirectUrl)
                ? Redirect($"/#/{redirectUrl}") : Redirect("/");
        }

        return RedirectToAction("Login");
    }

    public async Task<IActionResult> Logout()
    {
        userControlSession.ClearAll();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    private async Task<UserSession> GetEmployeeLogin(int userId)
    {
        var criteria = new GetEmployeeLoginByUserIdCriteria
        {
            UserId = userId
        };
        var userSession = await baseService.GetDataEntityAsync<UserSession>(criteria);
        if (userSession == null)
        {
            var user = await userService.GetByIdAsync(userId);
            userSession = new UserSession
            {
                EmployeeId = user.UserId,
                Code = user.Code,
                NickName = user.Nickname,
                FullName = user.FullName
            };
        }

        return userSession;
    }

    private string GetClientIP()
    {
        var clientIp = Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(clientIp))
        {
            var forwardedIps = clientIp.Split([','], StringSplitOptions.RemoveEmptyEntries);
            clientIp = forwardedIps[^1];
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