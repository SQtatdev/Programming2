using Xunit;
using KooliProjekt.Data;
using System.ComponentModel.DataAnnotations;
using KooliProjekt.Models;

namespace KooliProjekt.UnitTests.ModelTests
{
    public class PredictionTests
    {
        [Fact]
        public void Prediction_ValidModel_PassesValidation()
        {
            // Arrange
            var prediction = new Prediction
            {
                MatchId = 1,
                UserId = 1,
                PredictedScoreFirstTeam = 1,
                PredictedScoreSecondTeam = 2,
            };
            var context = new ValidationContext(prediction);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(prediction, context, results, true);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Prediction_InvalidResult_FailsValidation()
        {
            // Arrange
            var prediction = new Prediction
            {
                MatchId = 1,
                UserId = 3,
                PredictedScoreFirstTeam = 2,
                PredictedScoreSecondTeam =1,
            };
            var context = new ValidationContext(prediction);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(prediction, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage.Contains("формат результата"));
        }
    }
}