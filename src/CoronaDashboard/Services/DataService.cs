using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using CoronaDashboard.DataAccess.Mappers;
using CoronaDashboard.Models;
using CoronaDashboard.Models.Rijksoverheid;
using Microsoft.Extensions.Configuration;

namespace CoronaDashboard.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IDataMapper _mapper;
        private readonly string _StichtingNICEBaseUrl;
        private readonly string _ApiGatewayCovid19Url;

        public DataService(HttpClient httpClient, IConfiguration configuration, IDataMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _StichtingNICEBaseUrl = configuration["StichtingNICEBaseUrl"];
            _ApiGatewayCovid19Url = configuration["ApiGatewayCovid19Url"];
        }

        public Task<List<DateValueEntry<int>>> GetIntakeCountAsync()
        {
            return _httpClient.GetFromJsonAsync<List<DateValueEntry<int>>>($"{_StichtingNICEBaseUrl}/covid-19/public/intake-count");
        }

        public async Task<AgeDistribution> GetAgeDistributionStatusAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<JsonElement[][][]>($"{_StichtingNICEBaseUrl}/covid-19/public/age-distribution-status");

            return _mapper.MapAgeDistribution(result);
        }

        public async Task<DiedAndSurvivorsCumulative> GetDiedAndSurvivorsCumulativeAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<DateValueEntry<int>[][]>($"{_StichtingNICEBaseUrl}/covid-19/public/died-and-survivors-cumulative");

            return _mapper.MapDiedAndSurvivorsCumulative(result);
        }

        public async Task<BehandelduurDistribution> GetBehandelduurDistributionAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<JsonElement[][][]>($"{_StichtingNICEBaseUrl}/covid-19/public/behandelduur-distribution");

            return _mapper.MapBehandelduurDistribution(result);
        }

        public async Task<IReadOnlyCollection<DateValueEntry<double>>> GetTestedGGDDailyTotalAsync()
        {
            var infectedPeopleTotal = await _httpClient.GetFromJsonAsync<TestedGGDDailyTotal>($"{_ApiGatewayCovid19Url}/coronadashboard-rijksoverheid-NL?dataset=infected_people_total");

            return _mapper.MapTestedGGDDailyTotal(infectedPeopleTotal);
        }
    }
}