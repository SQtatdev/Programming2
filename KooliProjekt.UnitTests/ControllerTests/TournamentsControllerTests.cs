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
    public class TournamentsControllerTests
    {
        private readonly Mock<ITournamentService> _mockService;
        private readonly TournamentsController _controller;

        public TournamentsControllerTests()
        {
            _mockService = new Mock<ITournamentService>();
            _controller = new TournamentsController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResultWithTournaments()
        {
            // Arrange
            var tournaments = new PagedResult<Tournament>
            {
                Results = new List<Tournament> { new Tournament { Id = 1 } }
            };
            _mockService.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(tournaments);

            // Act
            var result = await _controller.Index(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(tournaments, viewResult.Model);
        }

        // ... Existing GET action tests ...

        // POST Create Action Tests
        [Fact]
        public async Task CreatePost_ReturnsRedirectToAction_WhenModelValid()
        {
            // Arrange
            var validTournament = new Tournament
            {
                Name = "Valid Tournament",
                TournamentStart = DateTime.Now.AddDays(1),
                TournamentEnd = DateTime.Now.AddDays(2)
            };
            _mockService.Setup(x => x.Save(validTournament));

            // Act
            var result = await _controller.Create(validTournament);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.Verify(x => x.Save(validTournament), Times.Once);
        }

        [Fact]
        public async Task CreatePost_ReturnsView_WhenModelInvalid()
        {
            // Arrange
            var invalidTournament = new Tournament { Name = "" };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(invalidTournament);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidTournament, viewResult.Model);
            _mockService.Verify(x => x.Save(It.IsAny<Tournament>()), Times.Never);
        }

        [Fact]
        public async Task CreatePost_ReturnsView_WhenDatesInvalid()
        {
            // Arrange
            var invalidTournament = new Tournament
            {
                Name = "Test",
                TournamentStart = DateTime.Now,
                TournamentEnd = DateTime.Now.AddDays(-1) // End date before start
            };
            _controller.ModelState.AddModelError("", "End date must be after start date");

            // Act
            var result = await _controller.Create(invalidTournament);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidTournament, viewResult.Model);
        }

        // POST Edit Action Tests
        [Fact]
        public async Task EditPost_ReturnsRedirectToAction_WhenModelValid()
        {
            // Arrange
            var validTournament = new Tournament
            {
                Id = 1,
                Name = "Updated Tournament",
                TournamentStart = DateTime.Now.AddDays(1),
                TournamentEnd = DateTime.Now.AddDays(2)
            };
            _mockService.Setup(x => x.Save(validTournament));

            // Act
            var result = await _controller.Edit(1, validTournament);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.Verify(x => x.Save(validTournament), Times.Once);
        }

        [Fact]
        public async Task EditPost_ReturnsView_WhenModelInvalid()
        {
            // Arrange
            var invalidTournament = new Tournament { Id = 1, Name = "" };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Edit(1, invalidTournament);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidTournament, viewResult.Model);
            _mockService.Verify(x => x.Save(It.IsAny<Tournament>()), Times.Never);
        }

        [Fact]
        public async Task EditPost_ReturnsNotFound_WhenIdMismatch()
        {
            // Arrange
            var tournament = new Tournament { Id = 2 };

            // Act
            var result = await _controller.Edit(1, tournament);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_ReturnsNotFound_WhenTournamentNotExists()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, Name = "Test" };
            _mockService.Setup(x => x.Save(tournament));

            // Act
            var result = await _controller.Edit(1, tournament);

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
        public async Task CreatePost_ReturnsView_WhenTournamentDatesOverlap()
        {
            // Arrange
            var overlappingTournament = new Tournament
            {
                Name = "Overlapping",
                TournamentStart = DateTime.Now,
                TournamentEnd = DateTime.Now.AddDays(2)
            };
            _mockService.Setup(x => x.HasOverlappingTournaments(
                overlappingTournament.TournamentStart,
                overlappingTournament.TournamentEnd));
            _controller.ModelState.AddModelError("", "Tournament dates overlap with existing tournament");

            // Act
            var result = await _controller.Create(overlappingTournament);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(overlappingTournament, viewResult.Model);
        }
    }
}