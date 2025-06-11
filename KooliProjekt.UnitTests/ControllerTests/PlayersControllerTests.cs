using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using KooliProjekt.Models;
using KooliProjekt.Data;
using KooliProjekt.UnitTests.ControllerTests;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class PlayersControllerTests : ControllerTestBase
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfPlayers()
        {
            // Arrange
            _context.Players.Add(new Player
            {
                Id = 1,
                PlayerName = "Test Player",
                Team = new Team { Id = 1, TeamName = "Test Team" }
            });
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/Players");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Test Player", content);
        }

        [Fact]
        public async Task Create_ValidPlayer_RedirectsToIndex()
        {
            // Arrange
            var team = new Team { Id = 1, TeamName = "Test Team" };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            var formData = new Dictionary<string, string>
            {
                { "PlayerName", "New Player" },
                { "TeamId", "1" }
            };

            var content = new FormUrlEncodedContent(formData);

            // Act
            var response = await _client.PostAsync("/Players/Create", content);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("New Player", _context.Players.First().PlayerName);
        }
    }
}