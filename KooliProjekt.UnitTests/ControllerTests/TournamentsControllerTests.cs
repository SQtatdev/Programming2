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
            _mockService.Setup(x => x.List(It.IsAny<int>())).ReturnsAsync(tournaments);

            // Act
            var result = await _controller.Index(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(tournaments, viewResult.Model);
        }

        [Fact]
        public async Task Details_ReturnsNotFoundForInvalidId()
        {
            // Arrange
            _mockService.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync((Tournament)null);

            // Act
            var result = await _controller.Details(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}