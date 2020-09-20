using FluentAssertions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyRateApp.IntegrationTests
{
    public class AuthControllerTest : IntegrationTest
    {
        [Fact]
        public async Task GenerateApiKeyAsync_ShouldreturnNewKey()
        {
            var response = await GetApiKeyResponse();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNull();
        }
    }
}
