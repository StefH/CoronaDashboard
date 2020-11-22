using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorApp.Api.Models;
using CoronaDashboard.Models;
using CoronaDashboard.Models.Rijksoverheid;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace BlazorApp.Api
{
    public class ApiFunctions
    {
        private const string StichtingNICEBaseUrl = "https://stichting-nice.nl";
        private const string ApiGatewayCovid19Url = "https://stef.azure-api.net/covid-19";

        private ILogger<ApiFunctions> _logger;
        private HttpClient _httpClient;

        public ApiFunctions(ILogger<ApiFunctions> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        [FunctionName("PositiefGetestePerDagNL")]
        public async Task<IActionResult> GetPositiefGetestePerDagNLAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("HttpTrigger - PositiefGetestePerDagNL");

            var result = await _httpClient.GetFromJsonAsync<Covid19RootObject>("https://coronadashboard.rijksoverheid.nl/json/NL.json");

            return new SystemTextJsonResult(result.InfectedPeopleTotal);
        }

        [FunctionName("PositiefGetestePerDag")]
        public async Task<IActionResult> GetPositiefGetestePerDagAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("HttpTrigger - PositiefGetestePerDag");

            var infectedPeopleTotal = await _httpClient.GetFromJsonAsync<InfectedPeopleTotal>($"{ApiGatewayCovid19Url}/coronadashboard-rijksoverheid-NL?dataset=infected_people_total");

            return new SystemTextJsonResult(infectedPeopleTotal);
        }

        [FunctionName("IntakeCount")]
        public async Task<IActionResult> GetIntakeCountAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("HttpTrigger - IntakeCount");

            var result = await _httpClient.GetFromJsonAsync<List<DateValueEntry<int>>>($"{StichtingNICEBaseUrl}/covid-19/public/intake-count");

            return new SystemTextJsonResult(result);
        }

        [FunctionName("AgeDistributionStatus")]
        public async Task<IActionResult> GetAgeDistributionStatusAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("HttpTrigger - AgeDistributionStatus");

            var result = await _httpClient.GetFromJsonAsync<JsonElement[][][]>($"{StichtingNICEBaseUrl}/covid-19/public/age-distribution-status");

            return new SystemTextJsonResult(result);
        }

        [FunctionName("DiedAndSurvivorsCumulative")]
        public async Task<IActionResult> GetDiedAndSurvivorsCumulativeAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("HttpTrigger - DiedAndSurvivorsCumulative");

            var result = await _httpClient.GetFromJsonAsync<DateValueEntry<int>[][]>($"{StichtingNICEBaseUrl}/covid-19/public/died-and-survivors-cumulative");

            return new SystemTextJsonResult(result);
        }

        [FunctionName("BehandelduurDistribution")]
        public async Task<IActionResult> GetBehandelduurDistributionAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("HttpTrigger - BehandelduurDistribution");

            var result = await _httpClient.GetFromJsonAsync<JsonElement[][][]>($"{StichtingNICEBaseUrl}/covid-19/public/behandelduur-distribution");

            return new SystemTextJsonResult(result);
        }
    }
}