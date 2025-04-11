using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public abstract class ServiceTestBase
    {
        protected readonly ApplicationDbContext _context;

        public ServiceTestBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
        }
    }
}