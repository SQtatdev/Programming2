using Microsoft.EntityFrameworkCore;
using System.Net;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class TeamsControllerTests : ControllerTestBase
    {
        // Конструктор без параметров
        public TeamsControllerTests()
        {
            // Дополнительная инициализация при необходимости
        }

        [Fact]
        public async Task Create_ValidTeam_RedirectsToIndex()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                { "Name", "Test Team" }
            };

            var content = new FormUrlEncodedContent(formData);

            // Act
            var response = await _client.PostAsync("/Teams/Create", content);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("Test Team", _context.Teams.First().TeamName);
        }
    }
}