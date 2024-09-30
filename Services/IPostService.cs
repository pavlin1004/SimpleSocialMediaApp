using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services
{
    public interface IPostService
    { 
        public interface IPostService
        {
            public Task<Post> GetPostByIdAsync(string id);
            public Task<IEnumerable<Post>> GetAllPostsAsync();
            public Task AddPostAsync(Post post);
            public Task UpdatePostAsync(Post post);
            public Task DeletePostAsync(string id);
            public Task<ICollection<Post>> GetAllFriendsPostsByIdAsync(string id);


        }
    }
}