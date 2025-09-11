using Microsoft.Extensions.Configuration;

namespace SMS.EPA.Core.Services;

public static class ConfigurationManagerService
{
    private static IConfiguration _configuration;

    public static void SetConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static string GetAppSetting(string key)
    {
        return _configuration?[key] ?? string.Empty;
    }
}