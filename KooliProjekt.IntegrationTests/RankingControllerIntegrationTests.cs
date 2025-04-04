using Xunit;
using Microsoft.Extensions.DependencyInjection;
using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using KooliProjekt.IntegrationTests.Helpers;
using System.Threading.Tasks;

namespace KooliProjekt.IntegrationTests
{
    public class RankingControllerIntegrationTests : TestBase
    {
        private readonly ApplicationDbContext _context;

        public RankingControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _context = Factory.Services.GetRequiredService<ApplicationDbContext>();
        }

        [Fact]
        public async Task Index_ReturnsRankingsFromDatabase()
        {
            // Arrange
            _context.Rankings.Add(new Ranking { UserId = "user1", TotalPoints = 150 });
            await _context.SaveChangesAsync();

            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Ranking");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("150", content);
        }
    }
}