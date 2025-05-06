using SimpleSocialApp.Data.Models;
using SQLitePCL;

namespace SimpleSociaMedialApp.Services.Functional.Interfaces
{
    public interface IPostService
    {
        public Task<Post?> GetPostByIdAsync(string id);
        public Task<List<Post>> GetAllUserPostsAsync(string userId, int size = 5, int count = 0);
        public Task<List<Post>> GetAllUserFriendsPostsAsync(string userId, List<AppUser> friends);
        public Task AddPostAsync(Post post);
        public Task UpdatePostAsync(Post post);
        public Task DeletePostAsync(string postId);
        public Task<int> GetCommentsCountAsync(string postId);
        public Task<int> GetLikesCountAsync(string postId);
        public Task<List<Post>> GetAllUserFriendsPostsAsync(List<string> friendsIds);
        public Task AddPostsAsync(List<Post> posts);
        public Task<bool> AnyAsync();
        public Task<List<Post>> GetAllAsyncWithUserMediaAsync();
        public Task<List<Post>> GetAllAsyncWithUserMediaAsync(int size, int count);
    }
}