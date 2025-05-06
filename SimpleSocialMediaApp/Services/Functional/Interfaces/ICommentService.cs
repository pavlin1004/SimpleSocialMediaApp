using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data.Models;

namespace SimpleSociaMedialApp.Services.Functional.Interfaces
{
    public interface ICommentService
    {

        public Task<Comment?> GetCommentAsync(string commentId);
        public Task<ICollection<Comment>> GetAllPostComments(string postId);
        public Task CreateCommentAsync(Comment comment);
        public Task UpdateCommentAsync(Comment comment);
        public Task<int> GetLikesCountAsync(string commentId);
        public Task DeleteCommentAsync(Comment comment);
    }
}
