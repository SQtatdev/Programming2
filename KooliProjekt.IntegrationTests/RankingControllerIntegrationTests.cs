using Xunit;
using KooliProjekt.IntegrationTests.Helpers;
using System.Threading.Tasks;

namespace KooliProjekt.IntegrationTests
{
    public class RankingControllerIntegrationTests : TestBase
    {
        public RankingControllerIntegrationTests(TestApplicationFactory<Program> factory)
            : base(factory) // Добавлен параметр и вызов базового конструктора
        {
        }

        [Fact]
        public async Task Index_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await Client.GetAsync("/Ranking");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}