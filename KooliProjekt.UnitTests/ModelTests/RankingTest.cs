using Xunit;
using KooliProjekt.Models;

namespace KooliProjekt.UnitTests.ModelTests
{
    public class RankingTest
    {
        [Fact]
        public void Ranking_Properties_SetCorrectly()
        {
            // Arrange & Act
            var ranking = new Ranking
            {
                Id = 1,
                UserId = "1", // Исправлено: строка вместо int
                TotalPoints = 100,
            };

            // Assert
            Assert.Equal(1, ranking.Id);
            Assert.Equal("1", ranking.UserId); // Проверка строки
            Assert.Equal(100, ranking.TotalPoints);
        }
    }
}