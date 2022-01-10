using System.Threading.Tasks;
using Blazorise.Charts;
using CoronaDashboard.DataAccess.Models;

namespace CoronaDashboard.Services
{
    public interface IChartService
    {
        Task<DateRangeWithTodayValueDetails> GetTestedGGDAsync(LineChart<double?> chart);

        Task<DateRangeWithTodayValueDetails> GetIntakeCountAsync(LineChart<double?> chart);

        Task GetAgeDistributionStatusAsync(BarChart<int> chart);

        Task<DiedAndSurvivorsCumulativeDetails> GetDiedAndSurvivorsCumulativeAsync(LineChart<double> chart);

        Task GetBehandelduurDistributionAsync(BarChart<int> chart);
    }
}
