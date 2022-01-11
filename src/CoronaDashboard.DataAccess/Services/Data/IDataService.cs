using System.Collections.Generic;
using System.Threading.Tasks;
using CoronaDashboard.DataAccess.Models;

namespace CoronaDashboard.DataAccess.Services.Data
{
    public interface IDataService
    {
        Task<List<DateValueEntry<int>>> GetIntakeCountAsync();

        Task<AgeDistribution> GetAgeDistributionStatusAsync();

        Task<DiedAndSurvivorsCumulative> GetDiedAndSurvivorsCumulativeAsync();

        Task<BehandelduurDistribution> GetBehandelduurDistributionAsync();

        Task<IReadOnlyCollection<TestedGGD>> GetTestedGGDAsync();
    }
}