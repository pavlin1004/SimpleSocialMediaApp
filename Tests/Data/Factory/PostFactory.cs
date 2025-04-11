using SimpleSocialApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Data.Factory
{
    public static class PostFactory
    {
        public static List<Post> CreateAsync(int count, AppUser user)
        {
            var postList = new List<Post>();
            for(int i=0;i<count; i++)
            {
                postList.Add(
                    new Post
                    {
                        Id = $"{i}{user.Id}",
                        User = user,
                        Content = $"{i}",
                        CreatedDateTime = DateTime.Now,
                    }
               );
            }
            return postList;
        }

        public static Post CreateAsync(AppUser user)
        {
            var data = Guid.NewGuid().ToString();
            return new Post
            {
                Id = data,
                User = user,
                Content = data,
                CreatedDateTime = DateTime.Now,
            };
        }
    }
}
