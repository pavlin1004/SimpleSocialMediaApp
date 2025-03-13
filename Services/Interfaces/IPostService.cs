using SimpleSocialApp.Data.Models;
using SQLitePCL;

namespace SimpleSocialApp.Services.Interfaces
{
    public interface IPostService
    {
        public Task<Post?> GetPostByIdAsync(string id);
        public Task<ICollection<Post>> GetAllUserPostsAsync(string userId);
        public Task<ICollection<Post>> GetAllUserFriendsPostsAsync(string userId);
        public Task AddPostAsync(Post post);
        public Task UpdatePostAsync(Post post);
        public Task DeletePostAsync(string postId);
        public Task<int> GetCommentsCountAsync(string postId);
        public Task<int> GetLikesCountAsync(string postId);

        public Task<List<Post>> GetAllFriendsPosts(List<string> friends);

        public Task AddPostsAsync(List<Post> posts);
        public Task<bool> AnyAsync();
        public Task<List<Post>> GetAllAsync();
    }
}