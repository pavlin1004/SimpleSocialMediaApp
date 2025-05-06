using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSociaMedialApp.Services.Functional.Implementations;
using SimpleSociaMedialApp.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common;
using Tests.Data.Custom;
using Tests.Data.Factory;

namespace Tests.ServiceTests
{
    public class PostServiceTests : TestBase
    {
        private readonly PostService postService;

        public PostServiceTests()
        {
            postService = new PostService(context);
        }


        [Fact]
        public async Task ShouldGetPostByIdOrNullIfNoPost()
        {
            var post1 = await postService.GetPostByIdAsync("");

            await context.SeedAsync(new List<Post> { Posts.Post1 });
            var post2 = await postService.GetPostByIdAsync(Posts.Post1.Id);

            Assert.Null(post1);
            Assert.NotNull(post2);           
        }

        [Fact]
        public async Task ShouldFetchAllPostsForUser()
        {
            var user = AppUserFactory.CreateSingle();
            var posts = PostFactory.CreateList(5, user);

            await context.SeedAsync(posts);

            var fetchedPosts = await postService.GetAllUserPostsAsync(user.Id, 5, 0);

            Assert.True(fetchedPosts.Any());
            Assert.Equal(5,fetchedPosts.Count());
        }

        [Fact]
        public async Task ShouldGetPostOrderedByCreatedDateTime()
        {
            var user = AppUserFactory.CreateSingle();
            var firstPost = PostFactory.CreateSingle(user);
            var secondPost = PostFactory.CreateSingle(user);
            var thirdPost = PostFactory.CreateSingle(user);

            await context.SeedAsync([secondPost, thirdPost, firstPost]);

            var posts = await postService.GetAllUserPostsAsync(user.Id);

            Assert.NotNull(posts);
            Assert.Equal(thirdPost.CreatedDateTime,posts[0].CreatedDateTime);
            Assert.Equal(secondPost.CreatedDateTime,posts[1].CreatedDateTime);
            Assert.Equal(firstPost.CreatedDateTime,posts[2].CreatedDateTime);
        }

        [Fact]
        public async Task ShouldGetAllUserFriendsPosts()
        {
            var users = AppUserFactory.CreateList(3);

            var post1 = PostFactory.CreateSingle(users[0]);
            var post2 = PostFactory.CreateSingle(users[1]);
            var post3 = PostFactory.CreateSingle(users[2]);

            await context.SeedAsync(users)
                .SeedAsync(new List<Post> { post1, post2, post3 });


            var testData1 = await postService.GetAllUserFriendsPostsAsync(users[0].Id, new List<AppUser> { users[1], users[2] });
            var testData2 = await postService.GetAllUserFriendsPostsAsync(users[0].Id, new List<AppUser> { users[1]});


            Assert.True(testData1.Count() == 2);
            Assert.True(testData2.Count() == 1);
        }

        [Fact]
        public async Task ShouldGetAllUserFriendsPosts_PassingFriendsIds()
        {
            var users = AppUserFactory.CreateList(3);

            var post1 = PostFactory.CreateSingle(users[0]);
            var post2 = PostFactory.CreateSingle(users[1]);
            var post3 = PostFactory.CreateSingle(users[2]);

            await context.SeedAsync(users)
                .SeedAsync([post1, post2, post3]);


            var testData1 = await postService.GetAllUserFriendsPostsAsync([users[1].Id, users[2].Id]);
            var testData2 = await postService.GetAllUserFriendsPostsAsync([users[1].Id]);

            Assert.Equal(2,testData1.Count());
            Assert.Single(testData2);
        }

        [Fact]
        public async Task ShouldAddPostToTheDatabase()
        {
            var post = PostFactory.CreateSingle(AppUserFactory.CreateSingle());

            await postService.AddPostAsync(post);

            var tempData = await postService.GetPostByIdAsync(post.Id);

            Assert.NotNull(tempData);   
        }

        [Fact]
        public async Task ShouldUpdateExistingPostToTheDatabase()
        {
            var post = PostFactory.CreateSingle(AppUserFactory.CreateSingle());

            await postService.AddPostAsync(post);

            var tempData = await postService.GetPostByIdAsync(post.Id);

            Assert.NotNull(tempData);

            tempData.Content = "newData";
            await postService.UpdatePostAsync(tempData);
            var result = await postService.GetPostByIdAsync(post.Id);

            Assert.NotNull(result);
            Assert.Equal(result.Content, tempData.Content);
        }

        [Fact]
        public async Task ShouldDeleteExistingPostFromTheDatabase()
        {
            await context.SeedEntityAsync(Posts.Post1);
            await postService.DeletePostAsync(Posts.Post1.Id);

            var tempData = await postService.GetPostByIdAsync(Posts.Post1.Id);

            Assert.Null(tempData);       
        }

        [Fact]
        public async Task ShouldGetCorrectCommentCountForAPost()
        {
            await context.SeedEntityAsync(Posts.Post1);

            var commentCount = await postService.GetCommentsCountAsync(Posts.Post1.Id);

            Assert.Equal(1,commentCount);
        }

        [Fact]
        public async Task ShouldAddMultiplePostsAtOnce()
        {

            await postService.AddPostsAsync(new List<Post> { Posts.Post1, Posts.Post2 });

            var postCount = await context.Posts.CountAsync(); // Use async to ensure the query runs after data is committed

            Assert.Equal(2, postCount); 
        }
        [Fact]
        public async Task ShouldReturnFalseIfNoPostsInDatabase()
        {
            Assert.False(await postService.AnyAsync());
        }

        [Fact]
        public async Task ShouldReturnTrueIfAnyPostsInDatabase()
        {
            await context.SeedAsync(new List<Post> { Posts.Post1 });
            Assert.True(await postService.AnyAsync());
        }

        [Fact]
        public async Task ShouldGetAllPostIncludingOwnerAndHisProfilePicture()
        {
            await context.SeedAsync(new List<Post> { Posts.Post1 });
            var tempData1 = await postService.GetAllAsyncWithUserMediaAsync();
            var tempData2 = await postService.GetAllAsyncWithUserMediaAsync(1,0); // pagination


            Assert.NotNull(tempData1[0].User.Media);
            Assert.NotNull(tempData2[0].User.Media);
        }

        [Fact]
        public async Task ShouldGetLikesCountForAPost()
        {
            var user = AppUserFactory.CreateSingle();
            var post = PostFactory.CreateSingle(user);
            post.Reacts.Add(new Reaction() { User = user, ReactType = ReactType.Like });

            await context.SeedAsync(new List<Post> { post});

            var result = await postService.GetLikesCountAsync(post.Id);

            Assert.Equal(1,result);
        }

        




    }
}
