using System.Threading.Tasks;
using Xunit;
using KooliProjekt.Services;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.UnitTests.ControllerTests;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class RankingServiceTests : ControllerTestBase
    {
        [Fact]
        public async Task GetUserRanking_ReturnsCorrectRanking()
        {
            // Arrange
            _context.Rankings.Add(new Ranking
            {
                UserId = "1", // Исправлено: строка вместо int
                TotalPoints = 100
            });
            await _context.SaveChangesAsync();

            var service = new RankingServices(_context);

            // Act
            var ranking = service.GetUserRanking("1"); // Исправлено: передаем строку

            // Assert
            Assert.NotNull(ranking);
            Assert.Equal(100, ranking.TotalPoints);
        }
    }
}