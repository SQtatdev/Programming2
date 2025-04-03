using Xunit;
using Moq;
using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class RankingControllerTests
    {
        private readonly Mock<IRankingService> _mockService;
        private readonly RankingControllerTests _controller;

        public RankingControllerTests()
        {
            _mockService = new Mock<IRankingService>();
            _controller = new RankingController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResultWithRankings()
        {
            // Arrange
            var rankings = new List<Models.Ranking>
            {
                new Models.Ranking { Id = 1, UserId = "user1", Points = 100 }
            };
            _mockService.Setup(x => x.GetAllRankings()).ReturnsAsync(rankings);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(rankings, viewResult.Model as List<Ranking>);
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
    }
}