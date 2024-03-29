﻿using System.Net.Http;
using System.Threading.Tasks;
using Blazorise.Charts;
using CoronaDashboard;
using CoronaDashboard.DataAccess.Mappers;
using CoronaDashboard.DataAccess.Services.Data;
using CoronaDashboard.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var optionsMock = new Mock<IOptions<CoronaDashboardOptions>>();
            optionsMock.Setup(o => o.Value).Returns(new CoronaDashboardOptions
            {
                GroupByDays = 5,
                StichtingNICEBaseUrl = "https://stichting-nice.nl",
                ApiGatewayCovid19Url = "https://stef.azure-api.net/covid-19",
                GitHubMZelstAllDataUrl = "https://raw.githubusercontent.com/mzelst/covid-19/master/data/all_data.csv"
            });

            var dataService = new GetDataViaDirectCallsService(optionsMock.Object, new HttpClient(), new DataMapper());

            var ageDistributionStatus = await dataService.GetAgeDistributionStatusAsync();

            var diedAndSurvivorsCumulativeAsync = await dataService.GetDiedAndSurvivorsCumulativeAsync();

            var getBehandelduurDistributionAsync = await dataService.GetBehandelduurDistributionAsync();

            var testedGGD = await dataService.GetTestedGGDAsync();

            var dataMapper = new DataMapper();
            var chartService = new ChartService(optionsMock.Object, dataService, new BlazoriseInteropServices(null));

            var chart = new LineChart<double?>();
            var ggd = await chartService.GetTestedGGDAsync(chart);

            int x = 0;
        }
    }
}
