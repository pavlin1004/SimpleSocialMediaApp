using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services.Implementations;
using SimpleSociaMedialApp.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common;
using Tests.Data.Factory;

namespace Tests.ServiceTests
{
    public class CommentServiceTests : Initialise
    {
        private readonly CommentService commentService;

        public CommentServiceTests()
        {
            commentService = new CommentService(context);
        }

        [Fact]
        public async Task ShouldGetCommentById()
        {
            var user = AppUserFactory.CreateAsync();
            var post = PostFactory.CreateAsync(user);
            var comments = CommentFactory.CreateAsync(post, user, 2);

            await context.SeedAsync(comments);

            var result = await commentService.GetCommentAsync(comments[0].Id);

            Assert.NotNull(result);
            Assert.Equal(result.Id, comments[0].Id);
        }
        [Fact]
        public async Task ShouldGetAllCommentsForAPost()
        {
            var user = AppUserFactory.CreateAsync();
            var post = PostFactory.CreateAsync(user);
            var comments = CommentFactory.CreateAsync(post, user, 2);

            await context.SeedAsync(comments);

            var result = await commentService.GetAllPostComments(post.Id);

            Assert.NotNull(result);
            Assert.True(result.Count == 2);
        }

        [Fact]
        public async Task ShouldCreateACommentForAPost()
        {
            var users = AppUserFactory.CreateUsers(1);
            var posts = PostFactory.CreateAsync(1, users[0]);
            var comment = CommentFactory.CreateAsync(posts[0], users[0]);

            await commentService.CreateCommentAsync(comment);

            var result = await commentService.GetCommentAsync(comment.Id);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task ShouldUpdateCommentContent()
        {
            var users = AppUserFactory.CreateUsers(1);
            var posts = PostFactory.CreateAsync(1, users[0]);
            var comments = CommentFactory.CreateAsync(posts[0], users[0], 1);

            await context.SeedAsync(comments);

            var comment = await commentService.GetCommentAsync(comments[0].Id);

            Assert.NotNull(comment);

            comment.Content = "updated";

            await commentService.UpdateCommentAsync(comment);

            var result = await commentService.GetCommentAsync(comment.Id);

            Assert.NotNull(result);
            Assert.Equal("updated", result.Content);
        }

        [Fact]

        public async Task ShouldDeleteCommentFromDatabase()
        {
            var users = AppUserFactory.CreateUsers(1);
            var posts = PostFactory.CreateAsync(1, users[0]);
            var comments = CommentFactory.CreateAsync(posts[0], users[0], 1);

            await context.SeedAsync(comments);

            await commentService.DeleteCommentAsync(comments[0]);

            Assert.True(context.Comments.Count() == 0);
        }

        [Fact]

        public async Task ShouldGetLikesCountForAComment()
        {
            var users = AppUserFactory.CreateUsers(1);
            var posts = PostFactory.CreateAsync(1, users[0]);
            var comments = CommentFactory.CreateAsync(posts[0], users[0], 2);

            comments[1].Reacts.Add(new Reaction() { User = users[0], ReactType = ReactType.Like });
            await context.SeedAsync(comments);

            var result1 = await commentService.GetLikesCountAsync(comments[0].Id);
            var result2 = await commentService.GetLikesCountAsync(comments[1].Id);

            Assert.True(result1 == 0);
            Assert.True(result2 == 1);


        }
    }
}
