﻿using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;
using SimpleSociaMedialApp.Services.Functional.Interfaces;

namespace SimpleSociaMedialApp.Services.Functional.Implementations
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
                .Include(c => c.Media)
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public async Task<ICollection<Comment>> GetAllPostComments(string postId)
        {
            return await _context.Comments.
                Include(c => c.User).
                Include(c => c.Reacts).
                Where(c => c.PostId == postId).
                ToListAsync();
        }

        //public async Task<ICollection<Comment>> GetAllCommentChildComments(string commentId)
        //{
        //    return await _context.Comments
        //        .Where(c => c.Comments.Any(p =>p.ParentCommentId==commentId))
        //        .ToListAsync();
        //}
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
        public async Task<int> GetLikesCountAsync(string commentId)
        {
            return await _context.Reactions.Where(r => r.CommentId == commentId).CountAsync();
        }
    }
}
