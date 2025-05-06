using SimpleSocialApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Data.Factory
{
    public static class CommentFactory
    {
        public static Comment CreateSingle(Post post, AppUser user)
        {
            return new Comment()
            {
                Content = "test",
                CreatedDateTime = DateTime.Now,
                LikesCount = 0,
                Post = post,
                User = user
            };
        }
        public static List<Comment> CreateList(Post post, AppUser user, int count)
        {
            var commentList = new List<Comment>();
            for (int i = 0; i < count; i++)
            {
                commentList.Add( new Comment()
                {
                    Content = "test",
                    CreatedDateTime = DateTime.Now,
                    LikesCount = 0,
                    Post = post,
                    User = user
                });
            }
            return commentList;
        }
    }
}
