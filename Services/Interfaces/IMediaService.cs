using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services.Interfaces
{
    public interface IMediaService
    {
        public Task RemoveUserMediaAsync(string mediaId);

        public Task<Media> CreateAsync(Media media);
       
    }
}
