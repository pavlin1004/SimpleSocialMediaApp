
using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services
{
    public class MediaService : IMediaService
    {
        private readonly SocialDbContext _context;
        public MediaService(SocialDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Media media)
        {
            _context.Media.Add(media);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Media media)
        {
            _context.Media.Update(media);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Media media)
        {
            _context.Media.Remove(media);
            await _context.SaveChangesAsync();
        }

        public async Task<Media?> GetAsync(string id)
        {
            return await _context.Media.FindAsync(id);
        }

        public MediaType GetMediaType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            switch (extension)
            {

                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                    return  MediaType.Image; // Image types
                case ".mp4":
                case ".mov":
                case ".avi":
                    return  MediaType.Video; // Video types
                default:
                    return  MediaType.Other;
            }
        }
    }

}