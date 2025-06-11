using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Hosting;

namespace KooliProjekt.IntegrationTests.Helpers
{
    public class TestApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDB");
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();

                db.Database.EnsureCreated();
                SeedTestData(db);
            });
        }

        private void SeedTestData(ApplicationDbContext db)
        {
            var testUser = new ApplicationUser
            {
                Id = "test-user-id",
                UserName = "testuser@example.com",
                Email = "testuser@example.com"
            };
            db.Users.Add(testUser);

            // Исправлено: передаем правильные значения
            db.Rankings.Add(new Ranking
            {
                UserId = testUser.Id,
                TotalPoints = 100, // int вместо string
            });

            db.SaveChanges();
        }
    }
}