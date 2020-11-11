using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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
        private ILogger<ApiFunctions> _logger;
        private HttpClient _httpClient;

        public ApiFunctions(ILogger<ApiFunctions> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        [FunctionName("PositiefGetestePerDag")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("HttpTrigger - PositiefGetestePerDag");

            var result = await _httpClient.GetFromJsonAsync<Covid19RootObject>("https://coronadashboard.rijksoverheid.nl/json/NL.json");
            
            return new OkObjectResult(result.InfectedPeopleTotal);
        }
    }
}