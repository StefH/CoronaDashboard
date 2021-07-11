using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using CoronaDashboard.DataAccess.Mappers;
using CoronaDashboard.DataAccess.Models.GitHubMZelst;
using CoronaDashboard.DataAccess.Services;
using CoronaDashboard.Models;
using CoronaDashboard.Models.Rijksoverheid;
using CsvHelper;
using Microsoft.Extensions.Options;

namespace CoronaDashboard.Services.Data
{
    public class GetDataFromViaAzureFunctionService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IDataMapper _mapper;
        private readonly Lazy<Task<List<AllDataCsv>>> _allData;

        public GetDataFromViaAzureFunctionService(IOptions<CoronaDashboardOptions> options, HttpClient httpClient, IDataMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _allData = new Lazy<Task<List<AllDataCsv>>>(async () =>
            {
                var reader = await httpClient.GetStringAsync(options.Value.GitHubMZelstAllDataUrl);
                using var tr = new StringReader(reader);
                using var csv = new CsvReader(tr, CultureInfo.InvariantCulture);
                return csv.GetRecords<AllDataCsv>().ToList();
            });
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

        public async Task<IReadOnlyCollection<DateValueEntry<double>>> GetTestedGGDTotalAsyncOld()
        {
            var result = await _httpClient.GetFromJsonAsync<TestedGGDDailyTotal>("/api/TestedGGDDailyTotal");

            return _mapper.MapTestedGGDDailyTotal(result);
        }

        public async Task<IReadOnlyCollection<DateValueEntry<double>>> GetTestedGGDTotalAsync()
        {
            var data = await _allData.Value;
            return data.Select(csv => new DateValueEntry<double>
            {
                Date = csv.date,
                Value = csv.positivetests
            }).ToList();
        }
    }
}