using Xunit;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using KooliProjekt.Models;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class RankingServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly RankingServices _service;

        public RankingServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            SeedTestData();
            _service = new RankingServices(_context);
        }

        private void SeedTestData()
        {
            _context.Rankings.Add(new Ranking { Id = 1, UserId = 1, TotalPoints = 100 });
            _context.SaveChanges();
        }

        public void Dispose() => _context.Dispose();

        [Fact]
        public async Task GetById_ReturnsRanking()
        {
            var result = await _service.GetById(1);
            Assert.NotNull(result);
            Assert.Equal(100, result.TotalPoints);
        }

        [Fact]
        public async Task UpdateAllRankings_ExecutesSuccessfully()
        {
            await _service.UpdateAllRankings();
            Assert.True(true); // Just verify no exception thrown
        }
    }
}