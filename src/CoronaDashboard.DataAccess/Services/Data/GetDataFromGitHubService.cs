using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CoronaDashboard.DataAccess.Models;
using CoronaDashboard.DataAccess.Models.GitHubMZelst;
using CoronaDashboard.DataAccess.Options;
using CoronaDashboard.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;

namespace CoronaDashboard.Services.Data
{
    public abstract class GetDataFromGitHubService
    {
        private readonly Lazy<Task<List<AllDataCsv>>> _allData;

        public GetDataFromGitHubService(IOptions<CoronaDashboardDataAccessOptions> options, HttpClient httpClient)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture);

            _allData = new Lazy<Task<List<AllDataCsv>>>(async () =>
            {
                Console.WriteLine("@options.Value.GitHubMZelstAllDataUrl = " + options.Value.GitHubMZelstAllDataUrl);
                var @string = await httpClient.GetStringAsync(options.Value.GitHubMZelstAllDataUrl);
                Console.WriteLine("@string = " + @string);
                using var stringReader = new StringReader(@string);
                using var csvReader = new CsvReader(stringReader, config);

                try
                {
                    var records = csvReader.GetRecords<AllDataCsv>().ToList();
                    Console.WriteLine(records.Count);
                    return records;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
        }

        //public async Task<AgeDistribution> GetAgeDistributionStatusAsync()
        //{
        //    return new AgeDistribution();
        //}

        //public async Task<BehandelduurDistribution> GetBehandelduurDistributionAsync()
        //{
        //    return new BehandelduurDistribution();
        //}

        //public async Task<DiedAndSurvivorsCumulative> GetDiedAndSurvivorsCumulativeAsync()
        //{
        //    return new DiedAndSurvivorsCumulative();
        //}

        //public async Task<List<DateValueEntry<int>>> GetIntakeCountAsync()
        //{
        //    return new List<DateValueEntry<int>>();
        //}

        public async Task<IReadOnlyCollection<TestedGGD>> GetTestedGGDAsync()
        {
            var data = await _allData.Value;
            return data.Select(csv => new TestedGGD
            {
                Date = csv.Date,
                Positive = csv.PositiveTests,
                Total = csv.TestedTotal
            }).ToList();
        }
    }
}
