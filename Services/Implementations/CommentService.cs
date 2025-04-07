using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services.Interfaces;

namespace SimpleSocialApp.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly SocialDbContext _context;

        public CommentService(SocialDbContext context)
        {
            _context = context;
        }

        public async Task<Comment?> GetCommentAsync(string commentId)
        {
            return await _context.Comments
                .Include(c => c.Comments)
                .Include(c=>c.Media)
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public async Task<ICollection<Comment>> GetAllPostComments(string postId)
        {
            return await _context.Comments.
                Include(c =>c.User).
                Include(c => c.Reacts).
                Where(c => c.PostId == postId).
                ToListAsync();
        }

        public async Task<ICollection<Comment>> GetAllCommentChildComments(string commentId)
        {
            return await _context.Comments
                .Where(c => c.Comments.Any(p =>p.ParentCommentId==commentId))
                .ToListAsync();
        }
        public async Task CreateCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCommentAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCommentByIdAsync(string commentId)
        {
            var comment = await FindCommentToDelete(commentId);
            await DeleteCommentAsync(comment);
        }
        public async Task DeleteCommentAsync(Comment comment)
        {       
            var comments = comment.Comments.ToList();
            foreach (var child in comments)
            {
                await DeleteCommentAsync(child);
            }
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
        private async Task<Comment> FindCommentToDelete(string commentId)
        {
            var comment = await GetCommentAsync(commentId);
            if(comment == null)
            {
                throw new KeyNotFoundException("Comment you are trying to delete doesn't exist!");
            }
            return comment;
        }
        public async Task<int> GetLikesCountAsync(string commentId)
        {
            return await _context.Reactions.Where(r => r.CommentId == commentId).CountAsync();
        }
        public async Task<bool> AnyAsync()
        {
            return await _context.Comments.AnyAsync();
        }
    }
}
