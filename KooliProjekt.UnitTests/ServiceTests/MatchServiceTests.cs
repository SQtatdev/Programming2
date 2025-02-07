using Xunit;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;

public class MatchServiceTests
{
    // Получаем InMemory контекст для базы данных
    private ApplicationDbContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        return new ApplicationDbContext(options);
    }

    // Тест для метода Get
    [Fact]
    public async Task Get_ShouldReturnMatch_WhenIdExists()
    {
        // Arrange
        var context = GetDatabaseContext();
        var service = new MatchService(context);
        var match = new Match { Id = 1, FirstTeamId = 1, SecondTeamId = 2 };
        context.Matches.Add(match);
        await context.SaveChangesAsync();

        // Act
        var result = await service.Get(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    // Тест для метода List
    [Fact]
    public async Task List_ShouldReturnAllMatches()
    {
        // Arrange
        var context = GetDatabaseContext();
        var service = new MatchService(context);
        context.Matches.AddRange(
            new Match { Id = 1, FirstTeamId = 1, SecondTeamId = 2 },
            new Match { Id = 2, FirstTeamId = 3, SecondTeamId = 4 }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await service.List(1, 5, null);

        // Assert
        Assert.Equal(2, result.Results.Count());
    }

    // Тест для метода Save (новый объект)
    [Fact]
    public async Task Save_ShouldCreateNewMatch()
    {
        // Arrange
        var context = GetDatabaseContext();
        var service = new MatchService(context);
        var match = new Match { FirstTeamId = 1, SecondTeamId = 2 };

        // Act
        await service.Save(match);
        var savedMatch = await context.Matches.FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(savedMatch);
        Assert.Equal(1, savedMatch.FirstTeamId);
    }

    // Тест для метода Save (существующий объект)
    [Fact]
    public async Task Save_ShouldUpdateExistingMatch()
    {
        // Arrange
        var context = GetDatabaseContext();
        var service = new MatchService(context);
        var match = new Match { Id = 1, FirstTeamId = 1, SecondTeamId = 2 };
        context.Matches.Add(match);
        await context.SaveChangesAsync();

        // Act
        match.FirstTeamId = 3;
        await service.Save(match);
        var updatedMatch = await context.Matches.FindAsync(1);

        // Assert
        Assert.NotNull(updatedMatch);
        Assert.Equal(3, updatedMatch.FirstTeamId);
    }

    // Тест для метода Delete (существующий объект)
    [Fact]
    public async Task Delete_ShouldRemoveMatch_WhenIdExists()
    {
        // Arrange
        var context = GetDatabaseContext();
        var service = new MatchService(context);
        var match = new Match { Id = 1, FirstTeamId = 1, SecondTeamId = 2 };
        context.Matches.Add(match);
        await context.SaveChangesAsync();

        // Act
        await service.Delete(1);
        var deletedMatch = await context.Matches.FindAsync(1);

        // Assert
        Assert.Null(deletedMatch);
    }
}
