using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using CoronaDashboard.Models;
using CoronaDashboard.Models.Api;

namespace CoronaDashboard.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;

        public DataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<List<Entry>> GetIntakeCountAsync()
        {
            return _httpClient.GetFromJsonAsync<List<Entry>>("/covid-19/public/intake-count");
        }

        public async Task<AgeDistribution> GetAgeDistributionStatusAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<JsonElement[][][]>("/covid-19/public/age-distribution-status");

            return Map(result);
        }

        private static AgeDistribution Map(JsonElement[][][] data)
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
    }
}