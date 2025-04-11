using Xunit;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using KooliProjekt.Models;
using System;
using System.Linq;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class TournamentServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly TournamentService _service;

        public TournamentServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            SeedTestData();
            _service = new TournamentService(_context);
        }

        private void SeedTestData()
        {
            _context.Tournaments.AddRange(
                new Tournament
                {
                    Id = 1,
                    Name = "Champions League",
                    TournamentStart = DateTime.Now.AddDays(1),
                    TournamentEnd = DateTime.Now.AddDays(10)
                },
                new Tournament
                {
                    Id = 2,
                    Name = "Premier League",
                    TournamentStart = DateTime.Now.AddDays(-10),
                    TournamentEnd = DateTime.Now.AddDays(-1)
                });

            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task GetById_ReturnsTournament_WhenExists()
        {
            // Act
            var result = await _service.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Champions League", result.Name);
        }

        [Fact]
        public async Task GetById_ReturnsNull_WhenNotExists()
        {
            // Act
            var result = await _service.GetById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task List_ReturnsPagedTournaments()
        {
            // Act
            var result = await _service.List(1, 10);

            // Assert
            Assert.Equal(2, result.Results.Count);
            Assert.Equal(2, result.TotalCount);
        }

        [Fact]
        public async Task Save_CreatesNewTournament()
        {
            // Arrange
            var newTournament = new Tournament
            {
                Name = "New Tournament",
                TournamentStart = DateTime.Now.AddDays(5),
                TournamentEnd = DateTime.Now.AddDays(15)
            };

            // Act
            await _service.Save(newTournament);

            // Assert
            // Kontrolli läbi DbContexti
            var tournament = await _context.Tournaments.FirstOrDefaultAsync(t => t.Name == newTournament.Name);
            Assert.NotNull(tournament);
        }

        [Fact]
        public async Task Save_UpdatesExistingTournament()
        {
            // Arrange
            var tournament = await _context.Tournaments.FindAsync(1);
            tournament.Name = "Updated Name";

            // Act
            await _service.Save(tournament);

            // Assert
            var updated = await _context.Tournaments.FindAsync(1);
            Assert.Equal("Updated Name", updated.Name);
        }

        [Fact]
        public async Task Save_ThrowsException_WhenDatesInvalid()
        {
            // Arrange
            var invalidTournament = new Tournament
            {
                Name = "Invalid",
                TournamentStart = DateTime.Now.AddDays(5),
                TournamentEnd = DateTime.Now
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.Save(invalidTournament));
        }

        [Fact]
        public async Task Delete_RemovesTournament()
        {
            // Act
            await _service.Delete(1);

            // Assert
            Assert.Null(await _context.Tournaments.FindAsync(1));
            Assert.Single(await _context.Tournaments.ToListAsync());
        }

        [Fact]
        public async Task Delete_DoesNothing_WhenNotExists()
        {
            // Act
            await _service.Delete(999);

            // Assert
            Assert.Equal(2, await _context.Tournaments.CountAsync());
        }

        [Fact]
        public async Task TournamentExists_ReturnsTrue_WhenExists()
        {
            // Act
            var result = await _service.Exists(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task TournamentExists_ReturnsFalse_WhenNotExists()
        {
            // Act
            var result = await _service.Exists(999);

            // Assert
            Assert.False(result);
        }
    }
}