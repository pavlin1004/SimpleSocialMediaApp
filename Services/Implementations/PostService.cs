using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Data;
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Services.Interfaces;
using System.Net.WebSockets;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SQLitePCL;
using Microsoft.Identity.Client;

namespace SimpleSocialApp.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly SocialDbContext _context;
        private readonly IMediaService _mediaService;
        private readonly IUserService _userService;

        public PostService(SocialDbContext context, IMediaService mediaService, IUserService userService, IFriendshipService friendService)
        {
            _context = context;
            _mediaService = mediaService;
            _userService = userService;

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
        public async Task<ICollection<Post>> GetAllUserPostsAsync(string userId)
        {
            return await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Reacts)
                .Include(p => p.User)
                .Include(p => p.Media)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }
        public async Task<ICollection<Post>> GetAllUserFriendsPostsAsync(string userId)
        {
            var friends = await _userService.GetAllUserFriendsAsync(userId);
            var ids = friends.Select(x => x.Id).ToList();
            return await _context.Posts.Include(p => p.Media).Where(p => ids.Contains(p.UserId)).OrderByDescending(x => x.PostedOn).ToListAsync();
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
                //await _reactService.DeletePostReactsAsync(postId);
                //await _mediaService.DeleteMediaByPostIdAsync(postId);
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
    }
}        

