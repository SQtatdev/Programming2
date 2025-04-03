﻿using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net;
using KooliProjekt;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace KooliProjekt.IntegrationTests
{
    public class TournamentsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly ApplicationDbContext _context;

        public TournamentsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TournamentIntegrationDb");
                    });

                    var sp = services.BuildServiceProvider();
                    _context = sp.GetRequiredService<ApplicationDbContext>();
                    _context.Database.EnsureCreated();
                });
            });
        }

        [Fact]
        public async Task Create_ValidTournament_RedirectsToIndex()
        {
            // Arrange
            var client = _factory.CreateClient();
            var formData = new Dictionary<string, string>
            {
                { "Name", "New Tournament" },
                { "StartDate", DateTime.Now.AddDays(1).ToString() },
                { "EndDate", DateTime.Now.AddDays(10).ToString() }
            };

            // Act
            var response = await client.PostAsync("/Tournaments/Create",
                new FormUrlEncodedContent(formData));

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/Tournaments", response.Headers.Location.OriginalString);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}