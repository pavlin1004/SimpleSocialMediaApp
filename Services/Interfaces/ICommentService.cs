using Microsoft.EntityFrameworkCore;
using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Services.Interfaces
{
    public interface ICommentService
    {

        public Task<Comment?> GetCommentAsync(string commentId);
        public Task<ICollection<Comment>> GetAllPostComments(string postId);
        public Task<ICollection<Comment>> GetAllCommentChildComments(string commentId);
        public Task CreateCommentAsync(Comment comment);
        public Task UpdateCommentAsync(Comment comment);
        public Task DeleteCommentByIdAsync(string commentId);

        public Task DeleteCommentAsync(Comment comment);
    }
}
