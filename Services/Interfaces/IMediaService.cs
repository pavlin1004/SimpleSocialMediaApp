using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services.Interfaces
{
    public interface IMediaService
    {
        public Task RemoveUserMediaAsync(string mediaId);
    }
}
