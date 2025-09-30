using SMS.EPA.AutoService.Service.Implement;
using SMS.EPA.AutoService.Service.Interfaces;

namespace SMS.EPA.AutoService;

using System;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using SharpRaven;

public static class Program
{
    public static void Main()
    {
        log4net.Config.XmlConfigurator.Configure();
        try
        {
            LogManager
                .SetDefaultLogCategory(AppSetting.Sentry_Category)
                .Use(new SentryLogging(new SentryEngineOptions
                {
                    Dsn = new Dsn(AppSetting.Sentry_Dsn),
                    WriteLogTimeoutSeconds = AppSetting.Sentry_Timeout
                }));

            IEPAAutoService service = new EPAAutoService();
            service.Run();
        }
        catch (Exception ex)
        {
            LogService.Error(ex.Message, ex);
        }
    }
}