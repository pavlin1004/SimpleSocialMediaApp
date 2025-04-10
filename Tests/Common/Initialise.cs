using SimpleSocialApp.Data;
using SimpleSociaMedialApp.Tests.Common.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Common
{
    public class Initialise : IDisposable
    {
        public SocialDbContext context { get; private set; }

        public Initialise()
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
