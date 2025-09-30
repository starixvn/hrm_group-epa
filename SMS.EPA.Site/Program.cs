using System.IO;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Fanex.Data.SqlServer;
using Fanex.Logging.Sentry.RavenClient;
using GO.UserControl.Client.Core;
using GO.UserControl.Client.Core.Session;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SMS.EPA.Site;
using SMS.EPA.Site.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
        options.SerializerSettings.DateFormatString = "dd MMM, yyyy HH:mm:ss";
        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
    });

services.AddHttpContextAccessor();
services.AddHttpClient();
services.AddHttpClient("insecure-client")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = ServerCertificateCustomValidationCallback
    });
services.AddFanexLogSentry(configuration);
services.AddFanexSql(configuration);
services.AddCookieAuthentication(configuration);
services.AddRedis(configuration);
services.AddRedisSession(configuration);
services.AddDIServices(configuration);

var app = builder.Build();
var session = app.Services.GetRequiredService<IUserControlSession>();
if (session != null)
{
    UserControlClient.UseSession(session, configuration);
}

if (!app.Environment.IsDevelopment())
{
    app.UseMiddleware<ErrorHandler>();
    app.UseHsts();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();

var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});

// Serve static files from SourceApp directory
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "SourceApp")),
    RequestPath = "/SourceApp"
});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

const byte MinThreads = 100;

ThreadPool.SetMinThreads(MinThreads, MinThreads);

await app.RunAsync();
return;

bool ServerCertificateCustomValidationCallback(HttpRequestMessage message, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    => true;