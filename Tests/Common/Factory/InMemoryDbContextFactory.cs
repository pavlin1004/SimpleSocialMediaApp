using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;

namespace SimpleSociaMedialApp.Tests.Common.Factory
{
    public static class InMemoryDbContextFactory
    {
        public static SocialDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<SocialDbContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
             .Options;

            return new SocialDbContext(options);
        }

    }
}
