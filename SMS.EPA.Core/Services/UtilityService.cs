namespace SMS.EPA.Core.Services;

using System;
using SMS.EPA.Core.Services.Interfaces;
using Constants;

public class UtilityService() : IUtilityService
{
    public string GetAvatar(string code)
    {
        return "Avatars/avatar.jpg";
    }

    public double SplitNumber(double number, int numberOfDecimals)
    {
        var result = number;

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