using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services
{
    public interface IPostService
    { 
        public interface IPostService
        {
            Task<Post> GetPostByIdAsync(string id);
            Task<IEnumerable<Post>> GetAllPostsAsync();
            Task AddPostAsync(Post post);
            Task UpdatePostAsync(Post post);
            Task DeletePostAsync(string id);
        }
    }
}