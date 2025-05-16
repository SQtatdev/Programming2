using Xunit;
using Moq;
using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using KooliProjekt.Models;
using Prediction = KooliProjekt.Models.Prediction;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class PredictionsControllerTests
    {
        private readonly Mock<IPredictionService> _mockService;
        private readonly PredictionsController _controller;

        public PredictionsControllerTests()
        {
            _mockService = new Mock<IPredictionService>();
            _controller = new PredictionsController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResultWithPredictions()
        {
            // Arrange
            var predictions = new PagedResult<Prediction>
            {
                Results = new List<Prediction> { new Prediction { Id = 1 } }
            };
            _mockService.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(predictions);

            // Act
            var result = await _controller.Index(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(predictions, viewResult.Model);
        }

        // ... Existing GET action tests ...

        // POST Create Action Tests
        [Fact]
        public async Task CreatePost_ReturnsRedirectToAction_WhenModelValid()
        {
            // Arrange
            var validPrediction = new Prediction
            {
                MatchId = 1,
                PredictedScoreFirstTeam = 2,
                PredictedScoreSecondTeam = 1
            };
            _mockService.Setup(x => x.Save(validPrediction));

            // Act
            var result = await _controller.Create(validPrediction);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.Verify(x => x.Save(validPrediction), Times.Once);
        }

        [Fact]
        public async Task CreatePost_ReturnsView_WhenModelInvalid()
        {
            // Arrange
            var invalidPrediction = new Prediction();
            _controller.ModelState.AddModelError("MatchId", "Required");

            // Act
            var result = await _controller.Create(invalidPrediction);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidPrediction, viewResult.Model);
            _mockService.Verify(x => x.Save(It.IsAny<Prediction>()), Times.Never);
        }

        [Fact]
        public async Task CreatePost_ReturnsView_WhenScorePredictionInvalid()
        {
            // Arrange
            var invalidPrediction = new Prediction
            {
                MatchId = 1,
                PredictedScoreFirstTeam = -1, // Invalid score
                PredictedScoreSecondTeam = 1
            };
            _controller.ModelState.AddModelError("PredictedScoreFirstTeam", "Invalid score");

            // Act
            var result = await _controller.Create(invalidPrediction);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidPrediction, viewResult.Model);
        }

        // POST Edit Action Tests
        [Fact]
        public async Task EditPost_ReturnsRedirectToAction_WhenModelValid()
        {
            // Arrange
            var validPrediction = new Prediction
            {
                Id = 1,
                MatchId = 1,
                PredictedScoreFirstTeam = 3,
                PredictedScoreSecondTeam = 2
            };
            _mockService.Setup(x => x.Save(validPrediction)).ReturnsAsync(true);

            // Act
            var result = await _controller.Edit(1, validPrediction);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.Verify(x => x.Save(validPrediction), Times.Once);
        }

        [Fact]
        public async Task EditPost_ReturnsView_WhenModelInvalid()
        {
            // Arrange
            var invalidPrediction = new Prediction { Id = 1 };
            _controller.ModelState.AddModelError("MatchId", "Required");

            // Act
            var result = await _controller.Edit(1, invalidPrediction);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidPrediction, viewResult.Model);
            _mockService.Verify(x => x.Save(It.IsAny<Prediction>()), Times.Never);
        }

        [Fact]
        public async Task EditPost_ReturnsNotFound_WhenIdMismatch()
        {
            // Arrange
            var prediction = new Prediction { Id = 2 };

            // Act
            var result = await _controller.Edit(1, prediction);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // POST DeleteConfirmed Action Tests
        [Fact]
        public async Task DeleteConfirmed_ReturnsRedirectToAction_WhenSuccessful()
        {
            // Arrange
            int id = 1;
            _mockService.Setup(x => x.Delete(id)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteConfirmed(id);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.Verify(x => x.Delete(id), Times.Once);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsProblem_WhenException()
        {
            // Arrange
            int id = 1;
            _mockService.Setup(x => x.Delete(id)).ThrowsAsync(new Exception("Error"));

            // Act
            var result = await _controller.DeleteConfirmed(id);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            _mockService.Verify(x => x.Delete(id), Times.Once);
        }

        // Additional tests for business logic
        [Fact]
        public async Task CreatePost_ReturnsView_WhenPredictionAlreadyExists()
        {
            // Arrange
            var existingPrediction = new Prediction
            {
                MatchId = 1,
                UserId = 1
            };
            _controller.ModelState.AddModelError("", "Prediction already exists");

            // Act
            var result = await _controller.Create(existingPrediction);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(existingPrediction, viewResult.Model);
        }
    }
}