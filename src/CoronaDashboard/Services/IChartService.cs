using System.Threading.Tasks;
using Blazorise.Charts;

namespace CoronaDashboard.Services
{
    public interface IChartService
    {
        Task<string> GetIntakeCount(string label, LineChart<double> chart);

        Task GetAgeDistributionStatusAsync(BarChart<int> chart);
    }
}
