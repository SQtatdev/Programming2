using Xunit;
using Moq;
using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using KooliProjekt.Models;
using Match = KooliProjekt.Models.Match;
using KooliProjekt.Search;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class MatchesControllerTests
    {
        private readonly Mock<IMatchService> _mockService;
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly MatchesController _controller;

        public MatchesControllerTests()
        {
            _mockService = new Mock<IMatchService>();
            _mockContext = new Mock<ApplicationDbContext>();
            _controller = new MatchesController(_mockService.Object, _mockContext.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResultWithMatches()
        {
            // Arrange
            var matches = new PagedResult<Match>
            {
                Results = new List<Match> { new Match { Id = 1 } }
            };
            _mockService.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<MatchSearch>())).ReturnsAsync(matches);

            // Act
            var result = await _controller.Index(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(matches, viewResult.Model);
        }

        // ... Existing GET action tests ...

        // POST Create Action Tests
        [Fact]
        public async Task CreatePost_ReturnsRedirectToAction_WhenModelValid()
        {
            // Arrange
            var validMatch = new Match { FirstTeamId = 1, SecondTeamId = 2 };
            _mockService.Setup(x => x.Save(validMatch));

            // Act
            var result = await _controller.Create(validMatch);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.Verify(x => x.Save(validMatch), Times.Once);
        }

        [Fact]
        public async Task CreatePost_ReturnsView_WhenModelInvalid()
        {
            // Arrange
            var invalidMatch = new Match();
            _controller.ModelState.AddModelError("FirstTeamId", "Required");

            // Act
            var result = await _controller.Create(invalidMatch);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidMatch, viewResult.Model);
        }

        // POST Edit Action Tests
        [Fact]
        public async Task EditPost_ReturnsRedirectToAction_WhenModelValid()
        {
            // Arrange
            var validMatch = new Match { Id = 1, FirstTeamId = 1, SecondTeamId = 2 };
            _mockService.Setup(x => x.Save(validMatch)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(1, validMatch);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.Verify(x => x.Save(validMatch), Times.Once);
        }

        [Fact]
        public async Task EditPost_ReturnsView_WhenModelInvalid()
        {
            // Arrange
            var invalidMatch = new Match { Id = 1 };
            _controller.ModelState.AddModelError("FirstTeamId", "Required");

            // Act
            var result = await _controller.Edit(1, invalidMatch);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidMatch, viewResult.Model);
        }

        [Fact]
        public async Task EditPost_ReturnsNotFound_WhenIdMismatch()
        {
            // Arrange
            var match = new Match { Id = 2 };

            // Act
            var result = await _controller.Edit(1, match);

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
    }
}