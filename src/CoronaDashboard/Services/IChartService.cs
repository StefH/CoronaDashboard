using System.Threading.Tasks;
using Blazorise.Charts;

namespace CoronaDashboard.Services
{
    public interface IChartService
    {
        Task<string> GetIntakeCountAsync(LineChart<double> chart);

        Task GetAgeDistributionStatusAsync(BarChart<int> chart);

        Task<string> GetDiedAndSurvivorsCumulativeAsync(LineChart<int> chart);
    }
}
