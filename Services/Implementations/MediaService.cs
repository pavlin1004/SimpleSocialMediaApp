using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.CodeDom;
using SimpleSocialApp.Services.Interfaces;

namespace SimpleSocialApp.Services.Implementations
{
    public class MediaService : IMediaService
    {
        private readonly SocialDbContext _context;
        public MediaService(SocialDbContext context)
        {
            _context = context;
        }
        public async Task RemoveUserMediaAsync(string userId)
        {
            var media = await _context.Media.FirstOrDefaultAsync(m => m.UserId == userId);
            if (media != null)
            {
                _context.Media.Remove(media);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Media> CreateAsync(Media media)
        {
            await _context.Media.AddAsync(media);
            await _context.SaveChangesAsync();

            return media;
        }

        public async Task<bool> RemoveMediaForPostAsync(Post post)
        {
            if(post == null)
            {
                return false;
            }
            if (post.Media != null || post.Media.Count != 0)
            {
                _context.RemoveRange(post.Media);
                await _context.SaveChangesAsync();
            }
            return true;
          
        }

    }
}
