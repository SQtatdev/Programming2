using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Xunit;

namespace KooliProjekt.IntegrationTests.Helpers
{
    public abstract class TestBase : IClassFixture<TestApplicationFactory<Program>>
    {
        protected readonly HttpClient Client;
        protected readonly TestApplicationFactory<Program> Factory;

        protected TestBase(TestApplicationFactory<Program> factory)
        {
            Factory = factory;
            Client = factory.CreateClient();
        }

        protected ApplicationDbContext GetDbContext()
        {
            var scope = Factory.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }
    }
}