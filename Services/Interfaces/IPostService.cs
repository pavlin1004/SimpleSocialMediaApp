using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services.Interfaces
{
    public interface IPostService
    {
        public Task<Post?> GetPostByIdAsync(string id);
        public Task<IEnumerable<Post>> GetAllUserPostsAsync(string userId);
        public Task<IEnumerable<Post>> GetAllUserFriendsPostsAsync(string userId);
        public Task AddPostAsync(Post post);
        public Task UpdatePostAsync(Post post);
        public Task DeletePostAsync(string postId);

    }
}