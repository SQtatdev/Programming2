using Xunit;
using KooliProjekt.Data;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using KooliProjekt.Models;

namespace KooliProjekt.UnitTests.ModelTests
{
    public class TournamentTests
    {
        [Fact]
        public void Tournament_ValidModel_PassesValidation()
        {
            // Arrange
            var tournament = new Tournament
            {
                Name = "Valid Tournament",
                TournamentStart = DateTime.Now.AddDays(1),
                TournamentEnd = DateTime.Now.AddDays(10)
            };
            var context = new ValidationContext(tournament);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(tournament, context, results, true);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Tournament_InvalidDates_FailsValidation()
        {
            // Arrange
            var tournament = new Tournament
            {
                Name = "Invalid Tournament",
                TournamentStart = DateTime.Now.AddDays(10),
                TournamentEnd = DateTime.Now
            };

            // Act & Assert
            Assert.Throws<ValidationException>(() =>
                tournament.TournamentInfo);
        }
    }
}