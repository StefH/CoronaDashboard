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
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;

namespace CoronaDashboard.DataAccess.Services.Data
{
    public abstract class GetDataFromGitHubService
    {
        private readonly Lazy<Task<List<AllDataCsv>>> _allData;

        protected GetDataFromGitHubService(IOptions<CoronaDashboardDataAccessOptions> options, HttpClient httpClient)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture);

            _allData = new Lazy<Task<List<AllDataCsv>>>(async () =>
            {
                var @string = await httpClient.GetStringAsync(options.Value.GitHubMZelstAllDataUrl);
                using var stringReader = new StringReader(@string);
                using var csvReader = new CsvReader(stringReader, config);

                return csvReader.GetRecords<AllDataCsv>().ToList();
            });
        }

        public async Task<IReadOnlyCollection<TestedGGD>> GetTestedGGDAsync()
        {
            var data = await _allData.Value;
            return data.Select(csv => new TestedGGD
            {
                Date = csv.Date,
                Positive = FixPositiveTests(csv.PositiveTests),
                Tested = csv.TestedTotal
            }).ToList();
        }

        /// <summary>
        /// Fix for:
        /// 2022-02-04 	5052043 	0 	22406 	363924
        /// 2022-02-05 	4797157 	0 	21323 	-254886
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static double FixPositiveTests(double value)
        {
            if (value < 0)
            {
                return 100000;
            }

            if (value > 150000)
            {
                return 100000;
            }

            return value;
        }
    }
}