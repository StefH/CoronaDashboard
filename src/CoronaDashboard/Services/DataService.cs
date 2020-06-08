using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using CoronaDashboard.Models;

namespace CoronaDashboard.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;

        public DataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<List<Entry<int>>> GetIntakeCountAsync()
        {
            return _httpClient.GetFromJsonAsync<List<Entry<int>>>("/covid-19/public/intake-count");
        }

        public async Task<AgeDistribution> GetAgeDistributionStatusAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<JsonElement[][][]>("/covid-19/public/age-distribution-status");

            return MapAgeDistribution(result);
        }

        public async Task<DiedAndSurvivorsCumulative> GetDiedAndSurvivorsCumulativeAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<Entry<int>[][]>("/covid-19/public/died-and-survivors-cumulative");

            return MapDiedAndSurvivorsCumulative(result);
        }

        private static AgeDistribution MapAgeDistribution(JsonElement[][][] data)
        {
            return new AgeDistribution
            {
                Leeftijdsverdeling = data[0].Select(x => x[0].GetString()).ToArray(),
                NogOpgenomen = data[0].Select(x => x[1].GetInt32()).ToList(),
                ICVerlatenNogOpVerpleegafdeling = data[1].Select(x => x[1].GetInt32()).ToList(),
                ICVerlaten = data[2].Select(x => x[1].GetInt32()).ToList(),
                Overleden = data[3].Select(x => x[1].GetInt32()).ToList()
            };
        }

        private static DiedAndSurvivorsCumulative MapDiedAndSurvivorsCumulative(Entry<int>[][] data)
        {
            return new DiedAndSurvivorsCumulative
            {
                Overleden = data[0].ToList(),
                ICVerlatenNogOpVerpleegafdeling = data[1].ToList(),
                Verlaten = data[2].ToList()
            };
        }
    }
}