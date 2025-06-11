using Xunit;
using KooliProjekt.IntegrationTests.Helpers;
using System.Threading.Tasks;

namespace KooliProjekt.IntegrationTests
{
    public class TeamsControllerIntegrationTests : TestBase
    {
        public TeamsControllerIntegrationTests(TestApplicationFactory<Program> factory)
            : base(factory) // Добавлен параметр и вызов базового конструктора
        {
        }

        [Fact]
        public async Task Index_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await Client.GetAsync("/Teams");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}