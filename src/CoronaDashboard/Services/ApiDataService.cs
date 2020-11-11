using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CoronaDashboard.Models;

namespace CoronaDashboard.Services
{
    public class ApiDataService : IDataService
    {
        private readonly HttpClient _httpClient;

        public ApiDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<AgeDistribution> GetAgeDistributionStatusAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BehandelduurDistribution> GetBehandelduurDistributionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DiedAndSurvivorsCumulative> GetDiedAndSurvivorsCumulativeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<DateValueEntry<int>>> GetIntakeCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DateValueEntry<double>>> GetPositiefGetestePerDagAsync()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<DateValueEntry<double>>>("/api/PositiefGetestePerDag");
        }
    }
}