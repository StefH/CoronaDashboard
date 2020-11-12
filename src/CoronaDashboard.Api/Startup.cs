using System.IO;
using System.Net.Http;
using Api;
using CoronaDashboard.DataAccess.Mappers;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("local.settings.json", true, false)
               .AddEnvironmentVariables()
               .Build();

            // Register Serilog provider
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.ApplicationInsights(TelemetryConfiguration.CreateDefault(), TelemetryConverter.Traces)
                .ReadFrom.Configuration(config)
                .CreateLogger();
            builder.Services.AddLogging(lb => lb.AddSerilog(logger, dispose: true));

            builder.Services.AddSingleton(new HttpClient());

            builder.Services.AddSingleton<IDataMapper, DataMapper>();
        }
    }
}