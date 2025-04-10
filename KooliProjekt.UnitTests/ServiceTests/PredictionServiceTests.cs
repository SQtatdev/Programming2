using Xunit;
using Moq;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using KooliProjekt.Models;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class PredictionServiceTests
    {
        private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{System.Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task Save_AddsNewPredictionToDatabase()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var newPrediction = new Prediction { MatchId = 1, PredictedScoreFirstTeam = 1, PredictedScoreSecondTeam = 2 };
            var PredictedResult = Prediction.PredictedScoreFirstTeam + "," + Prediction.PredictedScoreSecondTeam;

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var service = new PredictionService(context);
                await service.Save(newPrediction);
            }

            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(1, context.Predictions.Count());
                Assert.Equal("1-0", context.Predictions.First().PredictedResult);
            }
        }

        [Fact]
        public async Task GetById_ReturnsCorrectPrediction()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var testId = 1;
            using (var context = new ApplicationDbContext(options))
            {
                context.Predictions.Add(new Prediction { Id = testId });
                await context.SaveChangesAsync();
            }

            // Act & Assert
            using (var context = new ApplicationDbContext(options))
            {
                var service = new PredictionService(context);
                var result = await service.GetById(testId);
                Assert.NotNull(result);
                Assert.Equal(testId, result.Id);
            }
        }
    }
}