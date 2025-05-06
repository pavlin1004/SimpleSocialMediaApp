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
using Tests.Data.Factory;

namespace Tests.ServiceTests
{
    public class CommentServiceTests : TestBase
    {
        private readonly CommentService commentService;

        public CommentServiceTests()
        {
            commentService = new CommentService(context);
        }

        [Fact]
        public async Task ShouldGetCommentById()
        {
            var user = AppUserFactory.CreateSingle();
            var post = PostFactory.CreateSingle(user);
            var comments = CommentFactory.CreateList(post, user, 2);

            await context.SeedAsync(comments);

            var result = await commentService.GetCommentAsync(comments[0].Id);

            Assert.NotNull(result);
            Assert.Equal(result.Id, comments[0].Id);
        }
        [Fact]
        public async Task ShouldGetAllCommentsForAPost()
        {
            var user = AppUserFactory.CreateSingle();
            var post = PostFactory.CreateSingle(user);
            var comments = CommentFactory.CreateList(post, user, 2);

            await context.SeedAsync(comments);

            var result = await commentService.GetAllPostComments(post.Id);

            Assert.NotNull(result);
            Assert.True(result.Count == 2);
        }

        [Fact]
        public async Task ShouldCreateACommentForAPost()
        {
            var user = AppUserFactory.CreateSingle();
            var posts = PostFactory.CreateSingle(user);
            var comment = CommentFactory.CreateSingle(posts, user);

            await commentService.CreateCommentAsync(comment);

            var result = await commentService.GetCommentAsync(comment.Id);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task ShouldUpdateCommentContent()
        {
            var user = AppUserFactory.CreateSingle();
            var post = PostFactory.CreateSingle(user);
            var comment = CommentFactory.CreateSingle(post, user);

            await context.SeedEntityAsync(comment);

            var testComment = await commentService.GetCommentAsync(comment.Id);

            Assert.NotNull(testComment);

            testComment.Content = "updated";

            await commentService.UpdateCommentAsync(testComment);

            var result = await commentService.GetCommentAsync(testComment.Id);

            Assert.NotNull(result);
            Assert.Equal("updated", result.Content);
        }

        [Fact]

        public async Task ShouldDeleteCommentFromDatabase()
        {
            var user = AppUserFactory.CreateSingle();
            var post = PostFactory.CreateSingle(user);
            var comment = CommentFactory.CreateSingle(post, user);

            await context.SeedEntityAsync(comment);

            await commentService.DeleteCommentAsync(comment);

            Assert.Empty(context.Comments);
        }

        [Fact]

        public async Task ShouldGetLikesCountForAComment()
        {
            var user = AppUserFactory.CreateSingle();
            var post = PostFactory.CreateSingle(user);
            var comments = CommentFactory.CreateList(post, user, 2);

            comments[1].Reacts.Add(new Reaction() { User = user, ReactType = ReactType.Like });
            await context.SeedAsync(comments);

            var result1 = await commentService.GetLikesCountAsync(comments[0].Id);
            var result2 = await commentService.GetLikesCountAsync(comments[1].Id);

            Assert.True(result1 == 0);
            Assert.True(result2 == 1);
        }
    }
}
