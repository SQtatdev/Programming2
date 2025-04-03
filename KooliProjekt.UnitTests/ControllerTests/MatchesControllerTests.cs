using Xunit;
using Moq;
using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using KooliProjekt.Models;
using Match = KooliProjekt.Models.Match; // Разрешение конфликта имен

// Удален using для Search, если такого пространства имен нет

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
            // Arrange
            int page = 1;
            var data = new List<Match>
            {
                new Match { Id = 1 /* другие свойства */ },
                new Match { Id = 2 /* другие свойства */ }
            };

            var pagedResult = new PagedResult<Match>
            {
                Items = data // или Results, в зависимости от реализации
            };

            // Если MatchSearch не существует, замените на null или создайте stub-класс
            _mockService.Setup(x => x.List(
                It.IsAny<int>(),  // page
                It.IsAny<int>(),  // pageSize
                null  // или создайте mock для MatchSearch, если он нужен
            )).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(pagedResult, viewResult.Model);
        }
    }
}