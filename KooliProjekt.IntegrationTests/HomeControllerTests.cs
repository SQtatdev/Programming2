using System.Net;
using System.Threading.Tasks;
using Xunit;
using KooliProjekt.IntegrationTests.Helpers;
using System.Net.Http;

namespace KooliProjekt.IntegrationTests
{
    public class HomeControllerTests : IClassFixture<TestApplicationFactory<Program>> // Переименовано
    {
        private readonly HttpClient _client;
        private readonly TestApplicationFactory<Program> _factory;

        public HomeControllerTests(TestApplicationFactory<Program> factory) // Переименовано
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_AnonymousCanAccessPrivacyPage()
        {
            // Arrange
            var url = "/Home/Privacy";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Index_ReturnRankingFromDatabase()
        {
            // Arrange
            var url = "/Home";

            // Act
            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("testuser@example.com", content);
            Assert.Contains("Position: 1", content);
        }
    }
}