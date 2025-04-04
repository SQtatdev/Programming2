using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net;
using KooliProjekt;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;
using KooliProjekt.IntegrationTests.Helpers;

namespace KooliProjekt.IntegrationTests
{
    public class TeamsControllerIntegrationTests : TestBase
    {
        public TeamsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
        }

        [Fact]
        public async Task Create_Team_RedirectsToIndex()
        {
            // Arrange
            var client = Factory.CreateClient();
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