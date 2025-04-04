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


namespace KooliProjekt.UnitTests.ControllerTests
{
    public class MatchesControllerTests
    {
        private readonly Mock<IMatchService> _mockService;
        private readonly Mock<Data.ApplicationDbContext> _mockContext;
        private readonly MatchesController _controller;

        public MatchesControllerTests()
        {
            _mockService = new Mock<IMatchService>();
            _mockContext = new Mock<Data.ApplicationDbContext>();
            _controller = new MatchesController(_mockService.Object, _mockContext.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResultWithMatches()
        {

            int page = 1;
            var data = new List<Match>
            {
                new Match { Id = 1 },
                new Match { Id = 2 }
            };

            var pagedResult = new PagedResult<Match>
            {
                Results = data
            };

            _mockService.Setup(x => x.List(
                It.IsAny<int>(), 
                It.IsAny<int>(), 
                null 
            )).ReturnsAsync(pagedResult);

            var result = await _controller.Index(page);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(pagedResult, viewResult.Model);
        }
    }
}