using Xunit;
using KooliProjekt.Data;
using System.ComponentModel.DataAnnotations;
using KooliProjekt.Models;

namespace KooliProjekt.UnitTests.ModelTests
{
    public class RankingTests
    {
        [Fact]
        public void Ranking_ValidModel_PassesValidation()
        {
            // Arrange
            var ranking = new Ranking
            {
                UserId = "user1",
                TotalPoints = 100,
            };
            var context = new ValidationContext(ranking);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(ranking, context, results, true);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Ranking_NegativePoints_FailsValidation()
        {
            // Arrange
            var ranking = new Ranking { TotalPoints = -1 };

            // Act & Assert
            Assert.Throws<ValidationException>(() => ranking.ValidatePoints());
        }
    }
}