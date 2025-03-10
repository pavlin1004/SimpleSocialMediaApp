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
            var media = await _context.Media.FirstOrDefaultAsync(x => x.User.Id == userId);
            if (media != null)
            {
                _context.Media.Remove(media);
                await _context.SaveChangesAsync();
            }
        }
    }
}
