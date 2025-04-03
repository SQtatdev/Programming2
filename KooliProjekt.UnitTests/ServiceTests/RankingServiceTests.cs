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
                context.Users.Add(new User { Id = "user1" });
                context.Predictions.AddRange(
                    new Prediction { UserId = "user1", PointsEarned = 10 },
                    new Prediction { UserId = "user1", PointsEarned = 20 });
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
                Assert.Equal(30, ranking.Points);
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
                    new Ranking { Points = 100 },
                    new Ranking { Points = 200 },
                    new Ranking { Points = 50 });
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