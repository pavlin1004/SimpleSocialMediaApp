
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services
{
    public interface IMediaService
    {
        public Task CreateAsync(Media media);

        public Task UpdateAsync(Media media);

        public Task RemoveAsync(Media media);

        public Task<Media?> GetAsync(string id);

        public MediaType GetMediaType(string fileName);
      
    }
}
