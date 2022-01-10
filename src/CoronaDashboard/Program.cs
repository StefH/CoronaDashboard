using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Bootstrap;
// using Blazorise.Icons.FontAwesome;
using CoronaDashboard.DataAccess.Mappers;
using CoronaDashboard.DataAccess.Options;
using CoronaDashboard.DataAccess.Services;
using CoronaDashboard.DataAccess.Services.Data;
using CoronaDashboard.Localization;
using CoronaDashboard.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace CoronaDashboard
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            // Example of loading a configuration as configuration isn't available yet at this stage.
            builder.Services.AddSingleton(provider =>
            {
                var config = provider.GetService<IConfiguration>();
                return Options.Create(config.Get<CoronaDashboardOptions>());
            });
            builder.Services.AddSingleton(provider =>
            {
                var config = provider.GetService<IConfiguration>();
                return Options.Create(config.Get<CoronaDashboardDataAccessOptions>());
            });

            // Blazorise
            builder.Services
                .AddBlazorise(options =>
                {
                    options.ChangeTextOnKeyPress = true;
                })
                .AddBootstrapProviders();
                //.AddFontAwesomeIcons();

            // Localization
            builder.Services.AddLocalization(options => options.ResourcesPath = "Localization");
            builder.Services.AddSingleton(typeof(IStringLocalizer), typeof(StringLocalizer<Resources>));

            // HttpClient
            var baseAddress = builder.HostEnvironment.BaseAddress;
            Console.WriteLine("baseAddress = " + baseAddress);

            bool isLocalHost = baseAddress.Contains("localhost");
            Console.WriteLine("isLocalHost = " + isLocalHost);

            bool isAzure = baseAddress.Contains("azurestaticapps.net") || baseAddress.Contains("coronadashboard.heyenrath.nl");
            Console.WriteLine("isAzure = " + isAzure);

            string httpClientBaseAddress = isLocalHost ? "http://localhost:7071" : baseAddress;
            Console.WriteLine("httpClientBaseAddress = " + httpClientBaseAddress);
            builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(httpClientBaseAddress) });

            // Services
            bool useApi = isAzure || isLocalHost;
            Console.WriteLine("useApi = " + useApi);
            if (useApi)
            {
                builder.Services.AddScoped<IDataService, GetDataFromViaAzureFunctionService>();
            }
            else
            {
                builder.Services.AddScoped<IDataService, GetDataViaDirectCallsService>();
            }
            builder.Services.AddSingleton<IDataMapper, DataMapper>();
            builder.Services.AddScoped<IChartService, ChartService>();
            builder.Services.AddScoped<JavaScriptInteropService>();
            builder.Services.AddScoped<BlazoriseInteropServices>();

            var host = builder.Build();
            //host.Services.UseBootstrapProviders();

            await host.RunAsync();
        }
    }
}
