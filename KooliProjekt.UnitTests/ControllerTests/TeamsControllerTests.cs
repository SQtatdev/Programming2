using Xunit;
using Moq;
using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
            _mockService.Setup(x => x.List(It.IsAny<int>())).ReturnsAsync(teams);

            // Act
            var result = await _controller.Index(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(teams, viewResult.Model);
        }

        [Fact]
        public async Task Details_ReturnsNotFoundForNullId()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}