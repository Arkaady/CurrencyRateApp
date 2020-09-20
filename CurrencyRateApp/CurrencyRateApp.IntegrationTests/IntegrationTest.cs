using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyRateApp.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

        public IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            TestClient = appFactory.CreateClient();
        }

        protected async Task<HttpResponseMessage> GetApiKeyResponse()
        {
            return await TestClient.GetAsync("api/Auth");
        }

        protected async Task SetApiKey()
        {
            var response = await GetApiKeyResponse();
            var apiKey =  await response.Content.ReadAsStringAsync();

            TestClient.DefaultRequestHeaders.Add("ApiKey", apiKey);
        }
    }
}
