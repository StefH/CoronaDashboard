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
        private const string StichtingNICEBaseUrl = "https://stichting-nice.nl";
        private readonly HttpClient _httpClient;

        public ApiDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<AgeDistribution> GetAgeDistributionStatusAsync()
        {
            return null;
        }

        public Task<BehandelduurDistribution> GetBehandelduurDistributionAsync()
        {
            return null;
        }

        public Task<DiedAndSurvivorsCumulative> GetDiedAndSurvivorsCumulativeAsync()
        {
            return null;
        }

        public Task<List<DateValueEntry<int>>> GetIntakeCountAsync()
        {
            return _httpClient.GetFromJsonAsync<List<DateValueEntry<int>>>($"{StichtingNICEBaseUrl}/covid-19/public/intake-count");
        }

        public Task<IEnumerable<DateValueEntry<double>>> GetPositiefGetestePerDagAsync()
        {
            return _httpClient.GetFromJsonAsync<IEnumerable<DateValueEntry<double>>>("/api/PositiefGetestePerDag");
        }
    }
}