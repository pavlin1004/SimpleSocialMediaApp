using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Data;
using Microsoft.EntityFrameworkCore;



namespace SimpleSocialApp.Services
{
    public class PostService
    {
        private readonly SocialDbContext _dbContext;

        public PostService(SocialDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Post?> GetPostByIdAsync(string id)
        {
            return await _dbContext.Posts
                                        .Include(p => p.Comments)
                                        .Include(p => p.Reacts)
                                        .Include(p => p.Media)
                                        .FirstOrDefaultAsync(p => p.Id == id);
 
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _dbContext.Posts
                                   .Include(p => p.Comments)
                                   .Include(p => p.Reacts)
                                   .Include(p => p.Media)
                                   .ToListAsync();
        }

        public async Task AddPostAsync(Post post)
        {
            _dbContext.Posts.Add(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(Post post)
        {
            _dbContext.Posts.Update(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletePostAsync(string id)
        {
            var post = await _dbContext.Posts.FindAsync(id);
            if (post != null)
            {
                _dbContext.Posts.Remove(post);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
