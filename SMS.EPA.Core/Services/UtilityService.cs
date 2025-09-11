using SMS.EPA.Core.Services.Interfaces;

namespace SMS.EPA.Core.Services;

using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using SMS.EPA.Core.Constants;

public class UtilityService(IHttpContextAccessor httpContextAccessor) : IUtilityService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string GetAvatar(string code)
    {
        //string fileName = $"{code}.jpg";

        //var webRootPath = _httpContextAccessor.HttpContext?.RequestServices
        //    .GetService(typeof(Microsoft.AspNetCore.Hosting.IWebHostEnvironment)) as Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
            
        //var avatarPath = Path.Combine(webRootPath?.WebRootPath ?? "", "avatars", fileName);
            
        //fileName = File.Exists(avatarPath)
        //    ? "avatars/" + fileName
        //    : "avatars/avatar.jpg";

        //return fileName;
        return "Avatars/avatar.jpg";
    }

    public double SplitNumber(double number, int numberOfDecimals)
    {
        double result = number;

        var decimals = numberOfDecimals <= 0
            ? Constants.NumberOfDecimalDefault
            : numberOfDecimals;

        if (Math.Abs(number % 1) > Constants.Tolerance)
        {
            const byte RaisingNumber = 10;
            result = Math.Truncate(number * Math.Pow(RaisingNumber, decimals)) / Math.Pow(RaisingNumber, decimals);
        }

        return result;
    }
}