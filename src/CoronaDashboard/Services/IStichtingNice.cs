using System.Collections.Generic;
using System.Threading.Tasks;
using CoronaDashboard.Models.Api;
using RestEase;

namespace CoronaDashboard.Services
{
    public interface IStichtingNice
    {
        [Get("/intake-count")]
        Task<List<Entry>> GetIntakeCountAsync();

        [Get("/age-distribution-status")]
        Task<object[][][]> GetAgeDistributionStatusAsync();
    }
}