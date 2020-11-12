using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Bootstrap;
using CoronaDashboard.DataAccess.Mappers;
using CoronaDashboard.Localization;
using CoronaDashboard.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace CoronaDashboard
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            // Blazorise
            builder.Services
                .AddBlazorise(options =>
                {
                    options.ChangeTextOnKeyPress = true;
                })
                .AddBootstrapProviders();

            // Localization
            builder.Services.AddLocalization(options => options.ResourcesPath = "Localization");
            builder.Services.AddSingleton(typeof(IStringLocalizer), typeof(StringLocalizer<Resources>));

            // HttpClient
            var baseAddress = builder.HostEnvironment.BaseAddress;
            builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(baseAddress) });

            // Services
            Console.WriteLine("baseAddress = " + baseAddress);
            bool useApi = baseAddress.Contains("azurestaticapps.net");
            Console.WriteLine("useApi = " + useApi);
            if (useApi)
            {
                builder.Services.AddScoped<IDataService, ApiDataService>();
            }
            else
            {
                builder.Services.AddScoped<IDataService, DataService>();
            }
            builder.Services.AddSingleton<IDataMapper, DataMapper>();
            builder.Services.AddScoped<IChartService, ChartService>();
            builder.Services.AddScoped<JavaScriptInteropService>();
            builder.Services.AddScoped<BlazoriseInteropServices>();

            var host = builder.Build();
            host.Services.UseBootstrapProviders();

            await host.RunAsync();
        }
    }
}
