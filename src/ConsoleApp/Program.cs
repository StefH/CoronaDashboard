using System.Net.Http;
using System.Threading.Tasks;
using CoronaDashboard.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["StichtingNICEBaseUrl"]).Returns("https://stichting-nice.nl");
            configMock.Setup(c => c["RIVMBaseUrl"]).Returns("https://data.rivm.nl/covid-19");

            var dataService = new DataService(new HttpClient(), configMock.Object);

            var ageDistributionStatus = await dataService.GetAgeDistributionStatusAsync();

            var diedAndSurvivorsCumulativeAsync = await dataService.GetDiedAndSurvivorsCumulativeAsync();

            var getBehandelduurDistributionAsync = await dataService.GetBehandelduurDistributionAsync();

            int x = 0;
        }
    }
}
