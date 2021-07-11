using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CoronaDashboard.DataAccess.Mappers;
using CoronaDashboard.DataAccess.Models.GitHubMZelst;
using CoronaDashboard.DataAccess.Services;
using CoronaDashboard.Models;
using CsvHelper;
using Microsoft.Extensions.Options;

namespace CoronaDashboard.Services.Data
{
    public class GetDataFromGitHubService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IDataMapper _mapper;
        private readonly Lazy<Task<List<AllDataCsv>>> _allData;

        public GetDataFromGitHubService(IOptions<CoronaDashboardOptions> options, HttpClient httpClient, IDataMapper mapper)
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
            return new AgeDistribution();
        }

        public async Task<BehandelduurDistribution> GetBehandelduurDistributionAsync()
        {
            return new BehandelduurDistribution();
        }

        public async Task<DiedAndSurvivorsCumulative> GetDiedAndSurvivorsCumulativeAsync()
        {
            return new DiedAndSurvivorsCumulative();
        }

        public async Task<List<DateValueEntry<int>>> GetIntakeCountAsync()
        {
            return new List<DateValueEntry<int>>();
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
