using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Data;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Services.Interfaces;
using System.Net.WebSockets;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SQLitePCL;
using Microsoft.Identity.Client;
using SimpleSocialApp.Data.Enums;
using CloudinaryDotNet.Actions;

namespace SimpleSocialApp.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly SocialDbContext _context;

        public PostService(SocialDbContext context)
        {
            _context = context;

        }
        public async Task<Post?> GetPostByIdAsync(string id)
        {
            return await _context.Posts
                .Include(x => x.Comments)
                .Include(x => x.Reacts)
                .Include(x => x.User)
                .Include(x => x.Media)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<List<Post>> GetAllUserPostsAsync(string userId, int size=5, int count=0)
        {
            var friendsPosts = await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Reacts)
                .Include(p => p.User)
                .Include(p => p.Media)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedDateTime)
                .ToListAsync();

            return friendsPosts.Skip(size * count).Take(size).ToList();
        }
        public async Task<List<Post>> GetAllUserFriendsPostsAsync(string userId, List<AppUser> friends)
        {
            var ids = friends.Select(x => x.Id).ToList();

            return await _context.Posts
                .Include(p => p.Media)
                .Where(p => ids.Contains(p.UserId))
                .OrderByDescending(x => x.CreatedDateTime)
                .ToListAsync();
        }
        public async Task AddPostAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }
        public async Task UpdatePostAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }
        public async Task DeletePostAsync(string postId)
        {
            var post = await GetPostByIdAsync(postId);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<int> GetCommentsCountAsync(string postId)
        {
            return await _context.Comments.Where(c => c.PostId == postId).CountAsync();          
        }

        public async Task<int> GetLikesCountAsync(string postId)
        {
            return await _context.Reactions.Where(r => r.PostId == postId).CountAsync();
        }
        public async Task<List<Post>> GetAllUserFriendsPostsAsync(List<string> friendsIds)
        {
            return await _context.Posts.Where(p => friendsIds.Contains(p.UserId)).ToListAsync();
        }

        public async Task AddPostsAsync(List<Post> posts)
        {
            await _context.Posts.AddRangeAsync(posts);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync()
        {
            return await _context.Posts.AnyAsync();
        }

        public async Task<List<Post>> GetAllAsyncWithUserMediaAsync()
        {
            return await _context.Posts
               .Include(p => p.Comments)
               .Include(p => p.Reacts)
               .Include(p => p.User)
               .ThenInclude(u=> u.Media)
               .Include(p => p.Media)
               .OrderByDescending(p => p.CreatedDateTime)
               .ToListAsync();
        }
        public async Task<List<Post>> GetAllAsyncWithUserMediaAsync(int size, int count)
        {
            var posts = await GetAllAsyncWithUserMediaAsync();
            return posts.Skip(count*size).Take(size).ToList();
        }

    }
}        

