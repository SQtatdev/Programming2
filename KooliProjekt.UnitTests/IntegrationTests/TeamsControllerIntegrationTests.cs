using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net;
using KooliProjekt;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.IntegrationTests
{
    public class TeamsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public TeamsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TeamsTestDb");
                    });
                });
            });
        }

        [Fact]
        public async Task Create_Team_RedirectsToIndex()
        {
            // Arrange
            var client = _factory.CreateClient();
            var formData = new Dictionary<string, string>
            {
                { "Name", "New Team" },
                { "Country", "Estonia" }
            };

            // Act
            var response = await client.PostAsync("/Teams/Create",
                new FormUrlEncodedContent(formData));

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/Teams", response.Headers.Location.OriginalString);
        }
    }
}