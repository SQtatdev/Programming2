using Xunit;
using Moq;
using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using KooliProjekt.Models;

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

        // Тест для Index
        [Fact]
        public async Task Index_ReturnsViewResultWithPredictions()
        {
            // Arrange
            var predictions = new List<Prediction>
            {
                new Prediction { Id = 1, UserId = 1 }
            };
            _mockService.Setup(x => x.List(It.IsAny<int>()))
                       .ReturnsAsync(new PagedResult<Prediction> { Results = predictions });

            // Act
            var result = await _controller.Index(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PagedResult<Prediction>>(viewResult.Model);
            Assert.Single(model.Results);
        }

        // Тест для Details
        [Fact]
        public async Task Details_ReturnsNotFoundForNullId()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Тест для Create (POST)
        [Fact]
        public async Task Create_ReturnsRedirectToIndex_WhenModelValid()
        {
            // Arrange
            var validPrediction = new Prediction { MatchId = 1, PredictedScoreFirstTeam = 2-1 };
            var validPrediction2 = new Prediction { MatchId = 1, PredictedScoreSecondTeam = 1-2 };

            // Act
            var result = await _controller.Create(validPrediction);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        // Тест для Edit (GET)
        [Fact]
        public async Task Edit_ReturnsViewWithPrediction()
        {
            // Arrange
            var testId = 1;
            _mockService.Setup(x => x.GetById(testId))
                       .ReturnsAsync(new Prediction { Id = testId });

            // Act
            var result = await _controller.Edit(testId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
        }
    }
}