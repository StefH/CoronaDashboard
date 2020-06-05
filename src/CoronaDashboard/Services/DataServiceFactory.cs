using System.Net.Http;
using RestEase;

namespace CoronaDashboard.Services
{
    public class DataServiceFactory : IDataServiceFactory
    {
        private readonly HttpClient _httpClient;

        public DataServiceFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IStichtingNice GetClient()
        {
            return RestClient.For<IStichtingNice>(_httpClient);
        }
    }
}