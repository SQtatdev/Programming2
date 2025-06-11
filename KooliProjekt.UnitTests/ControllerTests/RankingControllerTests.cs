using System.Threading.Tasks;
using Xunit;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using KooliProjekt.Models;
using KooliProjekt.Services;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class RankingControllerTests : ControllerTestBase
    {
        private readonly RankingsController _controller;

        public RankingControllerTests()
        {
            _controller = new RankingsController(new RankingServices(_context));
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithRankings()
        {
            // Arrange
            _context.Rankings.Add(new Ranking
            {
                UserId = "1", // Исправлено: строка вместо int
                TotalPoints = 100
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<List<Ranking>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            _context.Rankings.Add(new Ranking
            {
                Id = 1,
                UserId = "1", // Исправлено: строка вместо int
                TotalPoints = 100
            });
            await _context.SaveChangesAsync();

            var updatedRanking = new Ranking
            {
                Id = 1,
                UserId = "1", // Исправлено: строка вместо int
                TotalPoints = 200
            };

            // Act
            var result = await _controller.Edit(1, updatedRanking); // 1 - это int ID

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal(200, _context.Rankings.First().TotalPoints);
        }

        [Fact]
        public async Task Delete_ExistingId_RedirectsToIndex()
        {
            // Arrange
            _context.Rankings.Add(new Ranking
            {
                Id = 1,
                UserId = "1", // Исправлено: строка вместо int
                TotalPoints = 100
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteConfirmed(1); // 1 - это int ID

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Empty(_context.Rankings);
        }
    }
}