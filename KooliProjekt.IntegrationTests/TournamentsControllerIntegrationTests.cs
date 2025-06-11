using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using KooliProjekt.IntegrationTests.Helpers;
using KooliProjekt.Data;
using System.Net.Http;

namespace KooliProjekt.IntegrationTests
{
    public class TournamentsControllerIntegrationTests : TestBase
    {
        public TournamentsControllerIntegrationTests(TestApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        private ApplicationDbContext GetDbContext()
        {
            var scope = Factory.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        [Fact]
        public async Task Create_ValidTournament_RedirectsToIndex()
        {
            // Arrange
            using var context = GetDbContext();
            var initialCount = context.Tournaments.Count();

            // Исправлено: создание правильного FormUrlEncodedContent
            var formData = new Dictionary<string, string>
            {
                { "Name", "Test Tournament" },
                { "Location", "Test Location" }
                // Добавьте другие необходимые поля
            };

            var content = new FormUrlEncodedContent(formData);

            // Act
            var response = await Client.PostAsync("/Tournaments/Create", content);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal(initialCount + 1, context.Tournaments.Count());
        }
    }
}