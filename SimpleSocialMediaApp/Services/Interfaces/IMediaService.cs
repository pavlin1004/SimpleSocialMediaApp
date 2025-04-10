using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services.Interfaces
{
    public interface IMediaService
    {
        public Task RemoveUserMediaAsync(string mediaId);

        public Task<bool> RemoveMediaForPostAsync(Post post); // non-cascade delete
        public Task<Media> CreateAsync(Media media);
       
    }
}
