using System;
using System.Threading.Tasks;
using Fanex.Caching.Redis;
using Fanex.Data.Repository;
using Fanex.Logging;
using GO.UserControl.Client.Core.Services;
using GO.UserControl.Client.Core.Session;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMS.EPA.Core.Repositories;
using SMS.EPA.Core.Services;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Repositories;
using SMS.EPA.Repositories.Implement;
using SMS.EPA.Repositories.Interfaces;
using SMS.EPA.Services;
using SMS.EPA.Services.Implement;
using SMS.EPA.Services.Interfaces;
using SMS.EPA.Site.Middlewares;
using StackExchange.Redis;

namespace SMS.EPA.Site;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCookieAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = configuration.GetSection("AuthSettings:LoginPath").Value;
                options.LogoutPath = configuration.GetSection("AuthSettings:LogoutPath").Value;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(int.Parse(configuration.GetSection("AuthSettings:CookieTimeOut").Value));
                options.Cookie = new CookieBuilder
                {
                    Name = "HRM.Website.Cookies",
                    SecurePolicy = CookieSecurePolicy.SameAsRequest
                };
                options.Events.OnSigningOut = x =>
                {
                    x.Response.Redirect(options.LoginPath);
                    return Task.CompletedTask;
                };
            });

        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRedisCache(configuration.GetSection("fanex.caching.redis"));

        var redisConfig = configuration.GetSection("AuthSettings:RedisConfig").Value;
        var redisMultiplexer = ConnectionMultiplexer.Connect(redisConfig);

        services.AddDataProtection().PersistKeysToStackExchangeRedis(redisMultiplexer, "HRM.Website:ProtectionKeys");

        services.AddSingleton(redisMultiplexer);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfig;
            options.InstanceName = "HRM.Website:Session:";
        });

        return services;
    }

    public static IServiceCollection AddRedisSession(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSession(options =>
        {
            options.Cookie.IsEssential = true;
            options.IdleTimeout = TimeSpan.FromMinutes(int.Parse(configuration.GetSection("AuthSettings:SessionTimeOut").Value));
            options.Cookie.Name = "HRM.Website.Session";
        });

        return services;
    }

    public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigurationManagerService.SetConfiguration(configuration);

        services.AddSingleton<ILogger, Logger>();

        services.AddDIRepositories();

        services.AddSingleton<IUserControlSession, UserControlSession>();
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<ICryptoService, CryptoService>();
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IRoleService, RoleService>();
        services.AddSingleton<IFunctionService, FunctionService>();

        services.AddSingleton<IBaseService, BaseService>();
        services.AddSingleton<IBaseMailService, BaseMailService>();
        services.AddSingleton<IEmployeeService, EmployeeService>();
        services.AddSingleton<IUnitService, UnitService>();
        services.AddSingleton<IDepartmentService, DepartmentService>();
        
        services.AddSingleton<IDisciplineService, DisciplineService>();
        services.AddSingleton<ICommonService, CommonService>();
        services.AddSingleton<IDepartmentFlowService, DepartmentFlowService>();
        services.AddSingleton<IEmployeeEPAService, EmployeeEPAService>();
        services.AddSingleton<IEmployeeService, EmployeeService>();
        services.AddSingleton<IEPAHistoryService, EPAHistoryService>();
        services.AddSingleton<IEvaluatorByKPIService, EvaluatorByKPIService>();
        services.AddSingleton<IEvaluatorEPAService, EvaluatorEPAService>();
        services.AddSingleton<IFlowTemplateService, FlowTemplateService>();
        services.AddSingleton<IJobDescriptionService, JobDescriptionService>();
        services.AddSingleton<IKPIService, KPIService>();
        services.AddSingleton<IMailService, MailService>();
        services.AddSingleton<IManagementEPAService, ManagementEPAService>();
        services.AddSingleton<IPeriodMailService, PeriodMailService>();
        services.AddSingleton<IPeriodService, PeriodService>();
        services.AddSingleton<IPersonalFlowService, PersonalFlowService>();
        services.AddSingleton<IReviewerEPAService, ReviewerEPAService>();
        services.AddSingleton<IEmployeeEPARepository, EmployeeEPARepository>();
        services.AddSingleton<ISelfEvaluationService, SelfEvaluationService>();
        services.AddSingleton<ITargetEmployeeSettingService, TargetEmployeeSettingService>();
        services.AddSingleton<ITargetTemplateService, TargetTemplateService>();
        services.AddSingleton<IUnitService, UnitService>();
        services.AddDIReportServices();
        services.AddDIExcelServices();

        return services;
    }

    public static IServiceCollection AddDIRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<IBaseRepository, BaseRepository>();
        services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
        return services;
    }

    public static IServiceCollection AddDIReportServices(this IServiceCollection services)
    {
        services.AddSingleton<IReportRepository, ReportRepository>();

        return services;
    }

    public static IServiceCollection AddDIExcelServices(this IServiceCollection services)
    {
        services.AddSingleton<IReportService, ReportService>();

        return services;
    }
}