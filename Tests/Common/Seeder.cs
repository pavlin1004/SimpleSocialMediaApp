using Microsoft.CodeAnalysis.CSharp.Syntax;
using SimpleSocialApp.Data;

namespace SimpleSociaMedialApp.Tests.Common
{
    public static class Seeder
    {
        public static async Task<SocialDbContext> SeedAsync<T>(this SocialDbContext context, IEnumerable<T> data)
        {
            foreach (var item in data)
            {
                if(item == null) continue;
                await context.AddAsync(item);
            }
            await context.SaveChangesAsync();
            return context;
        }

        public static async Task<SocialDbContext> SeedAsync<T>(this Task<SocialDbContext> contextTask, IEnumerable<T> data)
        {
            var context = await contextTask;

            foreach (var item in data)
            {
                if (item == null) continue;
                await context.AddAsync(item);
            }
            await context.SaveChangesAsync();
            return context;
        }
    }
}
