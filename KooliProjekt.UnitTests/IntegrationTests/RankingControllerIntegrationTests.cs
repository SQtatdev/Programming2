using Xunit;
using Microsoft.Extensions.DependencyInjection;
using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Models;

namespace KooliProjekt.IntegrationTests
{
    public class RankingControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly ApplicationDbContext _context;

        public RankingControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("RankingIntegrationDb");
                    });

                    var sp = services.BuildServiceProvider();
                    _context = sp.GetRequiredService<ApplicationDbContext>();
                    _context.Database.EnsureCreated();
                });
            });
        }

        [Fact]
        public async Task Index_ReturnsRankingsFromDatabase()
        {
            // Arrange
            _context.Rankings.Add(new Ranking { UserId = "user1", Points = 150 });
            await _context.SaveChangesAsync();

            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Ranking");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("150", content);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}