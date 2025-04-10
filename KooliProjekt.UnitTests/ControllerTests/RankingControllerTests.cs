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
    public class RankingControllerTests
    {
        private readonly Mock<IRankingService> _mockService;
        private readonly RankingsController _controller;

        public RankingControllerTests()
        {
            _mockService = new Mock<IRankingService>();
            _controller = new RankingsController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResultWithRankings()
        {
            // Arrange
            var rankings = new List<Ranking>
            {
                new Ranking { Id = 1, UserId = "user1", TotalPoints = 100 }
            };
            var pagedResult = new PagedResult<Ranking>();
            pagedResult.Results = rankings;

            _mockService.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(pagedResult, viewResult.Model);
        }

        [Fact]
        public async Task UpdateRankings_CallsServiceMethod()
        {
            // Arrange
            _mockService.Setup(x => x.UpdateAllRankings()).Returns(Task.CompletedTask);

            // Act
            await _controller.UpdateRankings();

            // Assert
            _mockService.Verify(x => x.UpdateAllRankings(), Times.Once);
        }

        // ... Existing GET action tests ...

        // POST Create Action Tests
        [Fact]
        public async Task CreatePost_ReturnsRedirectToAction_WhenModelValid()
        {
            // Arrange
            var validRanking = new Ranking { UserId = "user1", TotalPoints = 100 };
            _mockService.Setup(x => x.Save(validRanking));

            // Act
            var result = await _controller.Create(validRanking);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.Verify(x => x.Save(validRanking), Times.Once);
        }

        [Fact]
        public async Task CreatePost_ReturnsView_WhenModelInvalid()
        {
            // Arrange
            var invalidRanking = new Ranking();
            _controller.ModelState.AddModelError("UserId", "Required");

            // Act
            var result = await _controller.Create(invalidRanking);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidRanking, viewResult.Model);
            _mockService.Verify(x => x.Save(It.IsAny<Ranking>()), Times.Never);
        }

        [Fact]
        public async Task CreatePost_ReturnsView_WhenPointsInvalid()
        {
            // Arrange
            var invalidRanking = new Ranking { UserId = "user1", TotalPoints = -10 };
            _controller.ModelState.AddModelError("TotalPoints", "Points cannot be negative");

            // Act
            var result = await _controller.Create(invalidRanking);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidRanking, viewResult.Model);
        }

        // POST Edit Action Tests
        [Fact]
        public async Task EditPost_ReturnsRedirectToAction_WhenModelValid()
        {
            // Arrange
            var validRanking = new Ranking { Id = 1, UserId = "user1", TotalPoints = 150 };
            _mockService.Setup(x => x.Save(validRanking)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(1, validRanking);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.Verify(x => x.Save(validRanking), Times.Once);
        }

        [Fact]
        public async Task EditPost_ReturnsView_WhenModelInvalid()
        {
            // Arrange
            var invalidRanking = new Ranking { Id = 1 };
            _controller.ModelState.AddModelError("UserId", "Required");

            // Act
            var result = await _controller.Edit(1, invalidRanking);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidRanking, viewResult.Model);
            _mockService.Verify(x => x.Save(It.IsAny<Ranking>()), Times.Never);
        }

        [Fact]
        public async Task EditPost_ReturnsNotFound_WhenIdMismatch()
        {
            // Arrange
            var ranking = new Ranking { Id = 2 };

            // Act
            var result = await _controller.Edit(1, ranking);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // POST DeleteConfirmed Action Tests
        [Fact]
        public async Task DeleteConfirmed_ReturnsRedirectToAction_WhenSuccessful()
        {
            // Arrange
            int id = 1;
            _mockService.Setup(x => x.Delete(id)).Returns(Task.CompletedTask);

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
        public async Task CreatePost_ReturnsView_WhenUserAlreadyRanked()
        {
            // Arrange
            var existingRanking = new Ranking { UserId = "user1" };
            _mockService.Setup(x => x.UserExists(existingRanking.UserId))
                       .ReturnsAsync(true);
            _controller.ModelState.AddModelError("", "User already has ranking");

            // Act
            var result = await _controller.Create(existingRanking);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(existingRanking, viewResult.Model);
        }
    }
}