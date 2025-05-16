using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Net.Http.Json; // Add this using directive

using KooliProjekt; // Ensure the namespace containing the Program class is referenced
using KooliProjekt.Data; // Add this using directive to reference the namespace containing ApplicationDbContext
using Microsoft.AspNetCore.Mvc.Testing; // Correct namespace for WebApplicationFactory
using System.Collections.Generic; // Add this using directive for List<T>
using Microsoft.Extensions.DependencyInjection;
[Collection("Sequential")]
public class PlayersControllerTests : ControllerTestBase
{
    public PlayersControllerTests(WebApplicationFactory<Program> factory)

        : base(factory) { }

    protected override void SeedTestData()
    {
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var _context = scopedServices.GetRequiredService<ApplicationDbContext>(); // Obtain the DbContext

        var team = new Team { Id = 1, TeamName = "Test Team" };
        var players = new List<Player>
        {
            new Player { Id = 1, PlayerName = "Player 1", TeamId = 1, Team = team },
            new Player { Id = 2, PlayerName = "Player 2", TeamId = 1, Team = team }
        };

        _context.Teams.Add(team);
        _context.Players.AddRange(players);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAll_ReturnsAllPlayers()
    {
        var response = await _client.GetFromJsonAsync<List<Player>>("/api/Players");
        Assert.Equal(2, response?.Count); // Added null-conditional operator
    }

    [Fact]
    public async Task GetById_ReturnsPlayer_WhenExists()
    {
        var response = await _client.GetFromJsonAsync<Player>("/api/Players/1");
        Assert.Equal("Player 1", response?.PlayerName); // Corrected property name and added null-conditional operator
    }
}
public abstract class ControllerTestBase
{
    protected readonly WebApplicationFactory<Program> _factory;
    protected readonly HttpClient _client; // Added this field

    protected ControllerTestBase(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(); // Initialize _client
    }

    protected abstract void SeedTestData();
}
