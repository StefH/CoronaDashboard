using System;
using System.Net.Http;
using System.Threading.Tasks;
using CoronaDashboard.Services;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var dataService = new DataService(new HttpClient { BaseAddress = new Uri("https://stichting-nice.nl") });

            var ageDistributionStatus = await dataService.GetAgeDistributionStatusAsync();

            var diedAndSurvivorsCumulativeAsync = await dataService.GetDiedAndSurvivorsCumulativeAsync();

            int x = 0;
        }
    }
}
