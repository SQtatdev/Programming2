using Xunit;
using Moq;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class MatchServiceTests
    {
        [Fact]
        public async Task List_ReturnsPagedMatches()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Matches.AddRange(
                    new Match { Id = 1 },
                    new Match { Id = 2 });
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new MatchService(context);

                // Act
                var result = await service.List(It.IsAny<int>(), It.IsAny<int>());

                // Assert
                Assert.Equal(2, result.Results.Count);
            }
        }
    }
}