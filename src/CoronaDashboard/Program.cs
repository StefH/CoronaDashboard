using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using CoronaDashboard.Localization;
//using CoronaDashboard.Localization;
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
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();

            // Localization
            builder.Services.AddLocalization(options => options.ResourcesPath = "Localization");
            builder.Services.AddSingleton(typeof(IStringLocalizer), typeof(StringLocalizer<Resources>));

            // HttpClient
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri("https://stichting-nice.nl") });

            // Services
            builder.Services.AddSingleton<IDataService, DataService>();
            builder.Services.AddSingleton<IChartService, ChartService>();
            builder.Services.AddScoped<JavaScriptInteropService>();

            var host = builder.Build();

            host.Services
                .UseBootstrapProviders()
                .UseFontAwesomeIcons();

            await host.RunAsync();
        }
    }
}
