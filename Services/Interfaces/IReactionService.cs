using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services.Interfaces
{
    public interface IReactionService
    {
        //public Task<Reaction?> GetReactionByIdAsync(string id);
        //public Task<ICollection<Reaction>> GetAllPostReactionsAsync(string postId);
        //public Task<ICollection<Reaction>> GetAllCommentReactionsAsync(string commentId);
        //public Task CreateReactionAsync(Reaction reaction);
        //public Task UpdateReactionAsync(Reaction reaction);    
        //public Task RemoveReactionAsync(string reactionId);

        public  Task AddLikeAsync(Reaction reaction);
        public  Task RemoveLikeAsync(Reaction reaction);   
        public  Task<Reaction?> SearchPostReactionAsync(string postId, string userId);
        public  Task<Reaction?> SearchCommentReactionAsync(string commentId, string userId);

        public Task<int> GetCommentReactionCountAsync(string commentId);

        public Task<int> GetPostReactionCountAsync(string postId);
       
    }
}
