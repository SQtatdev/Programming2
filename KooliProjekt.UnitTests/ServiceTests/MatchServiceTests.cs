using Xunit;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using KooliProjekt.Models;
using System;
using KooliProjekt.Search;


var search = new MatchSearch();
namespace KooliProjekt.UnitTests.ServiceTests
{
    public class MatchServiceTests : ServiceTestBase, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly MatchService _service;

        public MatchServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            SeedTestData();
            _service = new MatchService(_context);
        }

        private void SeedTestData()
        {
            _context.Matches.AddRange(
                new Match { Id = 1, FirstTeamId = 1, SecondTeamId = 2, Date = DateOnly.FromDateTime(DateTime.Now).AddDays(1) },
                new Match { Id = 2, FirstTeamId = 2, SecondTeamId = 1, Date = DateOnly.FromDateTime(DateTime.Now).AddDays(2) });
            _context.SaveChanges();
        }

        public void Dispose() => _context.Dispose();

        [Fact]
        public async Task GetById_ReturnsMatch()
        {
            var result = await _service.GetById(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task List_ReturnsAllMatches()
        {
            var search = new MatchSearch(); // Create a default MatchSearch object
            var result = await _service.List(1, 10, search); // Pass the search parameter
            Assert.Equal(2, result.Results.Count);
            return;
        }

        [Fact]
        public async Task Save_CreatesNewMatch()
        {
            var newMatch = new Match { FirstTeamId = 1, SecondTeamId = 2, Date = DateOnly.FromDateTime(DateTime.Now) };
            await _service.Save(newMatch);

            // Kontrolli DbContexti kaudu, kas salvestati ära

            //&&Assert.True(result > 0);
        }

        [Fact]
        public async Task Delete_RemovesMatch()
        {
            await _service.Delete(1);
            Assert.Null(await _context.Matches.FindAsync(1));
        }
    }
}