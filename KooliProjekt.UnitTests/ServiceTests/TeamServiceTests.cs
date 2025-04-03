using Xunit;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using KooliProjekt.Models;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class TeamServiceTests
    {
        private DbContextOptions<ApplicationDbContext> CreateContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Save_AddsNewTeamToDatabase()
        {
            // Arrange
            var options = CreateContextOptions();
            var newTeam = new Team { Name = "Test Team" };

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TeamService(context);
                await service.Save(newTeam);
            }

            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(1, await context.Teams.CountAsync());
                Assert.Equal("Test Team", (await context.Teams.FirstAsync()).Name);
            }
        }
    }
}