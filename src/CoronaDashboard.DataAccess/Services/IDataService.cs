using System.Collections.Generic;
using System.Threading.Tasks;
using CoronaDashboard.Models;

namespace CoronaDashboard.DataAccess.Services
{
    public interface IDataService
    {
        Task<List<DateValueEntry<int>>> GetIntakeCountAsync();

        Task<AgeDistribution> GetAgeDistributionStatusAsync();

        Task<DiedAndSurvivorsCumulative> GetDiedAndSurvivorsCumulativeAsync();

        Task<BehandelduurDistribution> GetBehandelduurDistributionAsync();

        Task<IReadOnlyCollection<DateValueEntry<double>>> GetTestedGGDDailyTotalAsync();
    }
}