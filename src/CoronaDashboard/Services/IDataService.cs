using System.Collections.Generic;
using System.Threading.Tasks;
using CoronaDashboard.Models;

namespace CoronaDashboard.Services
{
    public interface IDataService
    {
        Task<List<DateValueEntry<int>>> GetIntakeCountAsync();

        Task<AgeDistribution> GetAgeDistributionStatusAsync();

        Task<DiedAndSurvivorsCumulative> GetDiedAndSurvivorsCumulativeAsync();

        Task<BehandelduurDistribution> GetBehandelduurDistributionAsync();
    }
}