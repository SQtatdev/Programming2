using Xunit;
using KooliProjekt.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace KooliProjekt.UnitTests.ModelTests
{
    public class MatchTests
    {
        [Fact]
        public void Match_ValidModel_PassesValidation()
        {
            // Arrange
            var match = new Match
            {
                Id = 1,
                Date = DateOnly.FromDateTime(DateTime.Now),
                FirstTeam = new Team { TeamName = "Home Matches" },
                SecondTeam = new Team { TeamName = "Away Matches" },
            };

            var context = new ValidationContext(match);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(match, context, results, true);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Match_RequiredFields_FailValidationWhenEmpty()
        {
            // Arrange
            var match = new Match(); // Создаем пустой объект

            var context = new ValidationContext(match);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(match, context, results, true);

            // Assert
            Assert.False(isValid);
        }
    }
}