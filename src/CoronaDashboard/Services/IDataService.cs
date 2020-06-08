using System.Collections.Generic;
using System.Threading.Tasks;
using CoronaDashboard.Models;
using CoronaDashboard.Models.Api;

namespace CoronaDashboard.Services
{
    public interface IDataService
    {
        Task<List<Entry>> GetIntakeCountAsync();

        Task<AgeDistribution> GetAgeDistributionStatusAsync();
    }
}