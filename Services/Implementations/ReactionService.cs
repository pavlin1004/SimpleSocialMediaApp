using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services.Interfaces;
using System.ComponentModel.Design;

namespace SimpleSocialApp.Services.Implementations
{
    public class ReactionService : IReactionService
    {
        private readonly SocialDbContext _context;

        public ReactionService(SocialDbContext context)
        {
            _context = context;
        }

        //public async Task<Reaction?> GetReactionByIdAsync(string id)
        //{
        //    return await _context.Reactions.FindAsync(id);
        //}
        //public async Task<ICollection<Reaction>> GetAllPostReactionsAsync(string postId)
        //{
        //    return await _context.Reactions.Where(r => r.PostId == postId).ToListAsync();
        //}
        //public async Task<ICollection<Reaction>> GetAllCommentReactionsAsync(string commentId)
        //{
        //    return await _context.Reactions.Where(r => r.CommentId == commentId).ToListAsync();
        //}
        //public async Task CreateReactionAsync(Reaction reaction)
        //{
        //    _context.Reactions.Add(reaction);
        //    await _context.SaveChangesAsync();
        //}
        //public async Task UpdateReactionAsync(Reaction reaction)
        //{
        //    _context.Reactions.Update(reaction);
        //    await _context.SaveChangesAsync();
        //}
        //public async Task RemoveReactionAsync(string reactionId)
        //{
        //    var reaction = await GetReactionByIdAsync(reactionId);
        //    if (reaction == null)
        //    {
        //        throw new KeyNotFoundException("Reaction with that key does not exist!");
        //    }
        //    _context.Reactions.Remove(reaction);
        //    await _context.SaveChangesAsync();
        //}

        public async Task AddLikeAsync(Reaction reaction)
        {
            await _context.Reactions.AddAsync(reaction);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveLikeAsync(Reaction reaction)
        {
            
            _context.Reactions.Remove(reaction);
            await _context.SaveChangesAsync();
        }

        public async Task<Reaction?> SearchPostReactionAsync(string postId, string userId)
        {
            var reaction = await _context.Reactions.FirstOrDefaultAsync(r => r.UserId == userId && r.PostId == postId);
            return reaction;
        }

        public async Task<Reaction?> SearchCommentReactionAsync(string commentId, string userId)
        {
            var reaction = await _context.Reactions.FirstOrDefaultAsync(r => r.UserId == userId && r.CommentId == commentId);
            return reaction;
        }

        public async Task<int> GetCommentReactionCountAsync(string commentId)
        {
            return await _context.Reactions.Where(r => r.CommentId == commentId).CountAsync();
        }

        public async Task<int> GetPostReactionCountAsync(string postId)
        {
            return await _context.Reactions.Where(r => r.PostId == postId).CountAsync();
        }
        public async Task<bool> AnyAsync()
        {
            return await _context.Reactions.AnyAsync();
        }



    }
}
