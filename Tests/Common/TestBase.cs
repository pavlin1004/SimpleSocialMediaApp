using SimpleSocialApp.Data;
using SimpleSociaMedialApp.Tests.Common.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Common
{
    public class TestBase : IDisposable
    {
        public SocialDbContext context { get; private set; }

        public TestBase()
        {
            context = InMemoryDbContextFactory.CreateContext();
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
