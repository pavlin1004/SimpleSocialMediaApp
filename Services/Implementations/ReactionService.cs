using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services.Interfaces;

namespace SimpleSocialApp.Services.Implementations
{
    public class ReactionService : IReactionService
    {
        private readonly SocialDbContext _context;

        public ReactionService(SocialDbContext context)
        {
            _context = context;
        }

        public async Task<Reaction?> GetReactionByIdAsync(string id)
        {
            return await _context.Reactions.FindAsync(id);
        }
        public async Task<ICollection<Reaction>> GetAllPostReactionsAsync(string postId)
        {
            return await _context.Reactions.Where(r => r.PostId == postId).ToListAsync();
        }
        public async Task<ICollection<Reaction>> GetAllCommentReactionsAsync(string commentId)
        {
            return await _context.Reactions.Where(r => r.CommentId == commentId).ToListAsync();
        }
        public async Task CreateReactionAsync(Reaction reaction)
        {
            _context.Reactions.Add(reaction);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateReactionAsync(Reaction reaction)
        {
            _context.Reactions.Update(reaction);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveReactionAsync(string reactionId)
        {
            var reaction = await GetReactionByIdAsync(reactionId);
            if (reaction == null)
            {
                throw new KeyNotFoundException("Reaction with that key does not exist!");
            }
            _context.Reactions.Remove(reaction);
            await _context.SaveChangesAsync();
        }
    }
}
