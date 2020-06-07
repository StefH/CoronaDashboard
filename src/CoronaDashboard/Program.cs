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

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri("https://stichting-nice.nl/covid-19/public/") });

            builder.Services.AddSingleton<IDataServiceFactory, DataServiceFactory>();
            builder.Services.AddSingleton<IChartService, ChartService>();

            var host = builder.Build();

            host.Services
                .UseBootstrapProviders()
                .UseFontAwesomeIcons();

            await host.RunAsync();
        }
    }
}
