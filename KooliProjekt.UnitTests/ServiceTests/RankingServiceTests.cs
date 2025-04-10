using Xunit;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using KooliProjekt.Models;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class RankingServiceTests
    {
        private DbContextOptions<ApplicationDbContext> CreateContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"RankingTestDb_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task CalculateRankings_UpdatesPointsCorrectly()
        {
            // Arrange
            var options = CreateContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                context.Users.Add(new User { Name = "Alfred" });
                context.Predictions.AddRange(
                    new Prediction { UserId = 4, Punktid = 10 },
                    new Prediction { UserId = 2, Punktid = 20 });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var service = new RankingService(context);
                await service.UpdateAllRankings();
            }

            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                var ranking = await context.Rankings.FirstAsync();
                Assert.Equal(30, ranking.TotalPoints);
            }
        }

        [Fact]
        public async Task GetTopRankings_ReturnsCorrectCount()
        {
            // Arrange
            var options = CreateContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                context.Rankings.AddRange(
                    new Ranking { TotalPoints = 100 },
                    new Ranking { TotalPoints = 200 },
                    new Ranking { TotalPoints = 50 });
                await context.SaveChangesAsync();
            }

            // Act & Assert
            using (var context = new ApplicationDbContext(options))
            {
                var service = new RankingService(context);
                var result = await service.GetTopRankings(2);
                Assert.Equal(2, result.Count);
                Assert.Equal(200, result[0].Points);
            }
        }
    }
}