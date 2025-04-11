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
    public class TeamServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly TeamService _service;

        public TeamServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            SeedTestData();
            _service = new TeamService(_context);
        }

        private void SeedTestData()
        {
            _context.Teams.AddRange(
                new Team { Id = 1, TeamName = "Team A"},
                new Team {Id = 2, TeamName = "Team B" });

            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task GetById_ReturnsTeam_WhenExists()
        {
            // Act
            var result = await _service.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Team A", result.TeamName);
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
        public async Task List_ReturnsAllTeams()
        {
            // Act
            var result = await _service.List(1, 10);

            // Assert
            Assert.Equal(2, result.Results.Count);
            Assert.Equal(2, result.TotalCount);
        }

        [Fact]
        public async Task Save_CreatesNewTeam()
        {
            // Arrange
            var newTeam = new Team
            {
                TeamName = "New Team"
            };

            // Act
            await _service.Save(newTeam);

            var team = await _context.Teams.FirstOrDefaultAsync(t => t.TeamName == newTeam.TeamName);

            Assert.NotNull(team);
        }

        [Fact]
        public async Task Save_UpdatesExistingTeam()
        {
            // Arrange
            var team = await _context.Teams.FindAsync(1);
            team.TeamName = "Updated Team Name";

            // Act
            await _service.Save(team);

            // Assert
            var updated = await _context.Teams.FindAsync(1);
            Assert.Equal("Updated Team Name", updated.TeamName);
        }

        [Fact]
        public async Task Save_ThrowsException_WhenNameEmpty()
        {
            // Arrange
            var invalidTeam = new Team { TeamName = "" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.Save(invalidTeam));
        }

        [Fact]
        public async Task Delete_RemovesTeam()
        {
            // Act
            await _service.Delete(1);

            // Assert
            Assert.Null(await _context.Teams.FindAsync(1));
            Assert.Single(await _context.Teams.ToListAsync());
        }

        [Fact]
        public async Task Delete_DoesNothing_WhenNotExists()
        {
            // Act
            await _service.Delete(999);

            // Assert
            Assert.Equal(2, await _context.Teams.CountAsync());
        }

        [Fact]
        public async Task TeamExists_ReturnsTrue_WhenExists()
        {
            // Act
            var result = await _service.Exists(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task TeamExists_ReturnsFalse_WhenNotExists()
        {
            // Act
            var result = await _service.Exists(999);

            // Assert
            Assert.False(result);
        }
    }
}