using System.Net;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class PredictionsControllerTests_Integration_Get : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PredictionsControllerTests_Integration_Get(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetPredictions_ReturnsOk()
        {
            var response = await _client.GetAsync("/Predictions");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
