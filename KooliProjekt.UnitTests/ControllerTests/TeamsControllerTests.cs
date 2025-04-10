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
    public class TeamsControllerTests
    {
        private readonly Mock<ITeamService> _mockService;
        private readonly TeamsController _controller;

        public TeamsControllerTests()
        {
            _mockService = new Mock<ITeamService>();
            _controller = new TeamsController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResultWithTeams()
        {
            // Arrange
            var teams = new PagedResult<Team>
            {
                Results = new List<Team> { new Team { Id = 1 } }
            };
            _mockService.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(teams);

            // Act
            var result = await _controller.Index(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(teams, viewResult.Model);
        }

        // ... Existing GET action tests ...

        // POST Create Action Tests
        [Fact]
        public async Task CreatePost_ReturnsRedirectToAction_WhenModelValid()
        {
            // Arrange
            var validTeam = new Team { TeamName = "Valid Team",};
            _mockService.Setup(x => x.Save(validTeam));

            // Act
            var result = await _controller.Create(validTeam);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.Verify(x => x.Save(validTeam), Times.Once);
        }

        [Fact]
        public async Task CreatePost_ReturnsView_WhenModelInvalid()
        {
            // Arrange
            var invalidTeam = new Team { TeamName = "" };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(invalidTeam);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidTeam, viewResult.Model);
            _mockService.Verify(x => x.Save(It.IsAny<Team>()), Times.Never);
        }

        [Fact]
        public async Task CreatePost_ReturnsView_WhenTeamNameTooLong()
        {
            // Arrange
            var invalidTeam = new Team { TeamName = new string('A', 101) }; // 101 characters
            _controller.ModelState.AddModelError("Name", "Maximum length is 100");

            // Act
            var result = await _controller.Create(invalidTeam);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidTeam, viewResult.Model);
        }

        // POST Edit Action Tests
        [Fact]
        public async Task EditPost_ReturnsRedirectToAction_WhenModelValid()
        {
            // Arrange
            var validTeam = new Team { Id = 1, TeamName = "Updated Team" };
            _mockService.Setup(x => x.Save(validTeam)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(1, validTeam);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockService.Verify(x => x.Save(validTeam), Times.Once);
        }

        [Fact]
        public async Task EditPost_ReturnsView_WhenModelInvalid()
        {
            // Arrange
            var invalidTeam = new Team { Id = 1, TeamName = "" };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Edit(1, invalidTeam);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidTeam, viewResult.Model);
            _mockService.Verify(x => x.Save(It.IsAny<Team>()), Times.Never);
        }

        [Fact]
        public async Task EditPost_ReturnsNotFound_WhenIdMismatch()
        {
            // Arrange
            var team = new Team { TeamName = "legends", Id = 2 };

            // Act
            var result = await _controller.Edit(1, team);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_ReturnsNotFound_WhenTeamNotExists()
        {
            // Arrange
            var team = new Team { Id = 1, TeamName = "Test" };
            _mockService.Setup(x => x.Save(team));

            // Act
            var result = await _controller.Edit(1, team);

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

        // Additional tests for special cases
        [Fact]
        public async Task CreatePost_ReturnsView_WhenTeamAlreadyExists()
        {
            // Arrange
            var existingTeam = new Team { TeamName = "Existing Team" };
            _mockService.Setup(x => x.TeamExists(existingTeam.TeamName))
                       .ReturnsAsync(true);
            _controller.ModelState.AddModelError("", "Team already exists");

            // Act
            var result = await _controller.Create(existingTeam);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(existingTeam, viewResult.Model);
        }
    }
}