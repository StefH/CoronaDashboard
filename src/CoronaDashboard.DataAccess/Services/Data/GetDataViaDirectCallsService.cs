using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using CoronaDashboard.DataAccess.Mappers;
using CoronaDashboard.DataAccess.Models;
using CoronaDashboard.DataAccess.Models.Rijksoverheid;
using CoronaDashboard.DataAccess.Options;
using Microsoft.Extensions.Options;

namespace CoronaDashboard.DataAccess.Services.Data
{
    public class GetDataViaDirectCallsService : GetDataFromGitHubService, IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IDataMapper _mapper;
        private readonly string _StichtingNICEBaseUrl;
        private readonly string _ApiGatewayCovid19Url;

        public GetDataViaDirectCallsService(IOptions<CoronaDashboardDataAccessOptions> options, HttpClient httpClient, IDataMapper mapper)
            : base(options, httpClient)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _StichtingNICEBaseUrl = options.Value.StichtingNICEBaseUrl;
            _ApiGatewayCovid19Url = options.Value.ApiGatewayCovid19Url;
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

        public async Task<IReadOnlyCollection<TestedGGD>> GetTestedGGDAsyncOld()
        {
            var infectedPeopleTotal = await _httpClient.GetFromJsonAsync<TestedGGDDailyTotal>($"{_ApiGatewayCovid19Url}/coronadashboard-rijksoverheid-NL?dataset=tested_ggd");

            return _mapper.MapTestedGGD(infectedPeopleTotal);
        }
    }
}