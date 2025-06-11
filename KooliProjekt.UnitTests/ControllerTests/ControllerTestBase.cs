using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using System;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class ControllerTestBase : IDisposable
    {
        protected readonly HttpClient _client;
        protected readonly ApplicationDbContext _context;

        public ControllerTestBase()
        {
            // Создаем фабрику приложения
            var factory = new WebApplicationFactory<Program>();
            _client = factory.CreateClient();

            // Инициализация in-memory БД
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _client.Dispose();
        }
    }
}