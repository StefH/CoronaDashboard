using System.Threading.Tasks;
using Blazorise.Charts;
using CoronaDashboard.Models;

namespace CoronaDashboard.Services
{
    public interface IChartService
    {
        Task<DateRangeWithTodayValueDetails> GetIntakeCountAsync(LineChart<double?> chart);

        Task GetAgeDistributionStatusAsync(BarChart<int> chart);

        Task<string> GetDiedAndSurvivorsCumulativeAsync(LineChart<double> chart);

        Task GetBehandelduurDistributionAsync(BarChart<int> chart);

        Task<DateRangeWithTodayValueDetails> GetPositiefGetestePerDagAsync(LineChart<double?> chart);
    }
}
