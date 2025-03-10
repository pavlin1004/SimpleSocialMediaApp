using Microsoft.AspNetCore.Mvc;
using SimpleSocialApp.Data.Models;

namespace SimpleSocialApp.Models.ViewModels.Posts
{
    public class PostViewModel
    {
        public required Post Post { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public required int CommentsCount { get; set; }

        public required int LikesCount { get; set; }
    }
}
