using Xunit;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using KooliProjekt.Models;
using Moq;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class TournamentServiceTests
    {
        private DbContextOptions<ApplicationDbContext> CreateContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TournamentTestDb_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task CreateTournament_SavesToDatabase()
        {
            // Arrange
            var options = CreateContextOptions();
            var newTournament = new Tournament { Name = "World Cup" };

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TournamentService(context);
                await service.Save(newTournament);
            }

            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(1, await context.Tournaments.CountAsync());
                Assert.Equal("World Cup", (await context.Tournaments.FirstAsync()).Name);
            }
        }

        [Fact]
        public async Task List_ReturnsPagedTournaments()
        {
            // Arrange
            var options = CreateContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                context.Tournaments.AddRange(
                    new Tournament { Id = 1 },
                    new Tournament { Id = 2 });
                await context.SaveChangesAsync();
            }

            // Act & Assert
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TournamentService(context);
                var result = await service.List(It.IsAny<int>(), It.IsAny<int>());
                Assert.Equal(2, result.Results.Count);
            }
        }
    }
}