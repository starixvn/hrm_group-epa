namespace SMS.EPA.Core.Services.Interfaces;

public interface IUtilityService
{
    string GetAvatar(string code);

    double SplitNumber(double number, int numberOfDecimals);
}