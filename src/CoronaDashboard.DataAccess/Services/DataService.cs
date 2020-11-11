﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using CoronaDashboard.Models;
using CoronaDashboard.Models.Rijksoverheid;
using Microsoft.Extensions.Configuration;

namespace CoronaDashboard.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly string _StichtingNICEBaseUrl;
        private readonly string _ApiGatewayCovid19Url;

        public DataService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
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

            return MapAgeDistribution(result);
        }

        public async Task<DiedAndSurvivorsCumulative> GetDiedAndSurvivorsCumulativeAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<DateValueEntry<int>[][]>($"{_StichtingNICEBaseUrl}/covid-19/public/died-and-survivors-cumulative");

            return MapDiedAndSurvivorsCumulative(result);
        }

        public async Task<BehandelduurDistribution> GetBehandelduurDistributionAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<JsonElement[][][]>($"{_StichtingNICEBaseUrl}/covid-19/public/behandelduur-distribution");

            return MapBehandelduurDistribution(result);
        }

        public async Task<IEnumerable<DateValueEntry<double>>> GetPositiefGetestePerDagAsync()
        {
            var covid19 = await _httpClient.GetFromJsonAsync<InfectedPeopleTotal>($"{_ApiGatewayCovid19Url}/coronadashboard-rijksoverheid-NL?dataset=infected_people_total");
            return MapInfectedPeopleTotal(covid19);
        }

        private static IEnumerable<DateValueEntry<double>> MapInfectedPeopleTotal(InfectedPeopleTotal data)
        {
            return data.Values.Select(i => new DateValueEntry<double>
            {
                Date = DateTimeOffset.FromUnixTimeSeconds(i.DateOfReportUnix).DateTime,
                Value = i.InfectedDailyTotal
            });
        }

        private static BehandelduurDistribution MapBehandelduurDistribution(JsonElement[][][] data)
        {
            return new BehandelduurDistribution
            {
                LabelsDagen = data[0].Select(x => $"{x[0].GetInt32()}").ToArray(),
                ICVerlatenNogOpVerpleegafdeling = data[0].Select(x => x[1].GetInt32()).ToList(),
                NogOpgenomen = data[1].Select(x => x[1].GetInt32()).ToList(),
                ICVerlaten = data[2].Select(x => x[1].GetInt32()).ToList(),
                Overleden = data[3].Select(x => x[1].GetInt32()).ToList()
            };
        }

        private static AgeDistribution MapAgeDistribution(JsonElement[][][] data)
        {
            return new AgeDistribution
            {
                LabelsLeeftijdsverdeling = data[0].Select(x => x[0].GetString()).ToArray(),
                NogOpgenomen = data[0].Select(x => x[1].GetInt32()).ToList(),
                ICVerlatenNogOpVerpleegafdeling = data[1].Select(x => x[1].GetInt32()).ToList(),
                ICVerlaten = data[2].Select(x => x[1].GetInt32()).ToList(),
                Overleden = data[3].Select(x => x[1].GetInt32()).ToList()
            };
        }

        private static DiedAndSurvivorsCumulative MapDiedAndSurvivorsCumulative(DateValueEntry<int>[][] data)
        {
            return new DiedAndSurvivorsCumulative
            {
                Overleden = data[0].ToList(),
                Verlaten = data[1].ToList(),
                NogOpVerpleegafdeling = data[2].ToList(),
            };
        }
    }
}