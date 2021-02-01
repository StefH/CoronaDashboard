using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using CoronaDashboard.DataAccess.Mappers;
using CoronaDashboard.Models;
using CoronaDashboard.Models.Rijksoverheid;

namespace CoronaDashboard.Services
{
    public class ApiDataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IDataMapper _mapper;

        public ApiDataService(HttpClient httpClient, IDataMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<AgeDistribution> GetAgeDistributionStatusAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<JsonElement[][][]>("/api/AgeDistributionStatus");

            return _mapper.MapAgeDistribution(result);
        }

        public async Task<BehandelduurDistribution> GetBehandelduurDistributionAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<JsonElement[][][]>("/api/BehandelduurDistribution");

            return _mapper.MapBehandelduurDistribution(result);
        }

        public async Task<DiedAndSurvivorsCumulative> GetDiedAndSurvivorsCumulativeAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<DateValueEntry<int>[][]>("/api/DiedAndSurvivorsCumulative");

            return _mapper.MapDiedAndSurvivorsCumulative(result);
        }

        public Task<List<DateValueEntry<int>>> GetIntakeCountAsync()
        {
            return _httpClient.GetFromJsonAsync<List<DateValueEntry<int>>>("/api/IntakeCount");
        }

        public async Task<IReadOnlyCollection<DateValueEntry<double>>> GetTestedGGDDailyTotalAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<TestedGGDDailyTotal>("/api/TestedGGDDailyTotal");

            return _mapper.MapTestedGGDDailyTotal(result);
        }
    }
}