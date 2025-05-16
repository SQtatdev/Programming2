using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

[Collection("Sequential")]
public class TeamsControllerTests : ControllerTestBase
{
    private readonly DbContext _context;

    public TeamsControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
        _context = CreateTestDbContext();
    }

    private DbContext CreateTestDbContext()
    {
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        return new DbContext(options);
    }

    protected override void SeedTestData()
    {
        var teamsDbSet = _context.Set<Team>();
        teamsDbSet.AddRange(
            new Team { Id = 1, TeamName = "Team A" },
            new Team { Id = 2, TeamName = "Team B" }
        );
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAll_ReturnsSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/Teams");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_ForInvalidId()
    {
        var response = await _client.GetAsync("/api/Teams/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
