using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace SimpleSocialApp.Services
{
    public class MediaService : IMediaService
    {
        private readonly SocialDbContext _context;

        public MediaService(SocialDbContext context)
        {
            _context = context;
        }
        public async Task<Media?> GetMediaByIdAsync(string id)
       {
            return await _context.Media.FindAsync(id);
       }
        public async Task<List<Media>> GetAllCommentMediaAsync(string commentId)
        {
            return await _context.Media.Where(x => x.CommentId == commentId).ToListAsync();
        }
        public async Task<List<Media>> GetAllPostMediaAsync(string postId)
        {
            return await _context.Media.Where(x => x.PostId == postId).ToListAsync();
        }
        public async Task CreateMediaAsync(Media media)
        {
            _context.Media.Add(media);
            await _context.SaveChangesAsync();

        }
        public async Task UpdateMediaAsync(Media media)
        {
            _context.Media.Update(media);
            await _context.SaveChangesAsync();

        }
        public async Task DeleteMediaAsync(Media media)
        {
            _context.Media.Remove(media);
            await _context.SaveChangesAsync();
        }



    }
}
