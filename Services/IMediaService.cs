using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services
{
    public interface IMediaService
    {
        Task<Media?> GetMediaByIdAsync(string id);
        Task<List<Media>> GetAllCommentMediaAsync(string commentId);
        Task<List<Media>> GetAllPostMediaAsync(string postId);
        Task CreateMediaAsync(Media media); 
        Task UpdateMediaAsync(Media media); 
        Task DeleteMediaAsync(Media media); 
    }
}
