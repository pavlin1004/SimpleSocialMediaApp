using Microsoft.Identity.Client;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Migrations;
using SimpleSocialApp.Services.Implementations;
using SimpleSociaMedialApp.Tests.Common;
using SimpleSociaMedialApp.Tests.Common.Factory;
using SimpleSociaMedialApp.Tests.Data;
using System.Data;
using Xunit;

namespace SimpleSociaMedialApp.Tests.ServiceTests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUserById_ShouldGetUserBasedOnId()
        {
            var context = InMemoryDbContextFactory.CreateContext();
            var userService = new UserService(context);

            var users = new List<AppUser> { Users.User1 };
            await context.SeedAsync(users);

            var user = await userService.GetUserByIdAsync(Users.User1.Id);

            Assert.NotNull(user);
            Assert.Equal(user.Id, Users.User1.Id);
            Assert.Equal(user.Posts.First().Id, Users.User1.Posts.First().Id);
            Assert.Equal(user.Media, Users.User1.Media);
        }

        [Fact]
        public async Task Get_ShouldFetchUserAndTheChatsHeIsIn()
        {
            var context = InMemoryDbContextFactory.CreateContext();
            var userService = new UserService(context);

            var users = new List<AppUser> { Users.User1, Users.User2, Users.User3, Users.User4, Users.User5 };
            var chats = new List<Chat> { Chats.Chat1, Chats.Chat2 };
            await context.SeedAsync(users).SeedAsync(chats);

            var chatShouldBeEmpty = await userService.GetUserWithCommunicationDetailsAsync(Users.User5.Id);
            var chat_shouldBe1 = await userService.GetUserWithCommunicationDetailsAsync(Users.User4.Id);
            var chat_shouldBe2 = await userService.GetUserWithCommunicationDetailsAsync(Users.User1.Id);

            Assert.NotNull(chatShouldBeEmpty?.Chats);
            Assert.Empty(chatShouldBeEmpty.Chats);
            Assert.True(chat_shouldBe1?.Chats.Count() == 1);
            Assert.True(chat_shouldBe2?.Chats.Count() == 2);
        }

        [Fact]
        public async Task ShouldAddUserToTheDatabase()
        {
            var context = InMemoryDbContextFactory.CreateContext();
            var userService = new UserService(context);

            var user = Users.User1;
            var shouldNotBeInBase = await userService.GetUserByIdAsync(user.Id);

            await userService.CreateAsync(user);
            var shouldBeInBase = await userService.GetUserByIdAsync(user.Id);

            Assert.Null(shouldNotBeInBase);
            Assert.NotNull(shouldBeInBase);
            Assert.Equal(shouldBeInBase?.Id, user.Id);
        }

        [Fact]
        public async Task ShouldUpdateFirstNameForUser()
        {
            var context = InMemoryDbContextFactory.CreateContext();
            var userService = new UserService(context);

            const string email = "myNewEmail@gmail.com";

            var user = Users.User1;
            var currentEmail = user.Email;

            await userService.CreateAsync(user);

            user.Email = email;
            await userService.UpdateAsync(user);

            var updated = await userService.GetUserByIdAsync(user.Id);

            Assert.NotNull(updated);
            Assert.NotEqual(updated.Email, currentEmail);
            Assert.Equal(email, updated.Email);
        }
        [Fact]
        public async Task ShouldRemoveUserFromDatabase()
        {
            var context = InMemoryDbContextFactory.CreateContext();
            var userService = new UserService(context);

            await context.SeedAsync(new List<AppUser> { Users.User1 });
            await userService.RemoveAsync(Users.User1.Id);

            var user = await userService.GetUserByIdAsync(Users.User1.Id);

            Assert.Null(user);
        }

        [Fact]
        public async Task UserShouldHaveSpecificSubstringInTheFirstAndLastNameConcatenated()
        {
            var context = InMemoryDbContextFactory.CreateContext();
            var userService = new UserService(context);

            var users = new List<AppUser>() {Users.User1,Users.User2,Users.User3, Users.User4, Users.User5 };
            await context.SeedAsync(users);

            const string query1 = "ko";
            const string query2 = "ni";
            const string query3 = "test";

            var users_q1 = await userService.SearchUsersByNameAsync(query1);
            var users_q2 = await userService.SearchUsersByNameAsync(query2);
            var users_q3 = await userService.SearchUsersByNameAsync(query3);

            Assert.True(users_q1.Count() == 2);
            Assert.True(users_q2.Count() == 1);
            Assert.True(users_q3.Count() == 3);
        }

        [Fact]
        public async Task ShouldReturnFalseIfDbHasNoUsers()
        {
            var context = InMemoryDbContextFactory.CreateContext();
            var userService = new UserService(context);

            Assert.False(await userService.AnyAsync());
        }
        [Fact]
        public async Task ShouldReturnTrueIfDbHasAnyUsers()
        {
            var context = InMemoryDbContextFactory.CreateContext();
            var userService = new UserService(context);

            await context.SeedAsync(new List<AppUser> { Users.User1 });
            Assert.True(await userService.AnyAsync());
        }

        [Fact]
        public async Task ShouldAddProfilePictureForUser()
        {
            var context = InMemoryDbContextFactory.CreateContext();
            var userService = new UserService(context);

            await context.SeedAsync(new List<AppUser> { Users.User1 });
            var media = Users.User1.Media;

            const string mediaParams = "test";
            await userService.AddProfilePictureAsync(Users.User1,
                new Media {
                    Id = mediaParams,
                    PublicId = mediaParams,
                    Url = mediaParams, 
                    Type = MediaOptions.Image
                });


            var currentUser = await userService.GetUserByIdAsync(Users.User1.Id);

            Assert.NotNull(currentUser);
            Assert.NotNull(currentUser.Media);
            Assert.NotEqual(currentUser.Media, media);
            Assert.Equal(mediaParams, currentUser.Media.Url);
        }
        [Fact]
        public async Task ShouldGetAllUsersCount()
        {
            var context = InMemoryDbContextFactory.CreateContext();
            var userService = new UserService(context);

            await context.SeedAsync(new List<AppUser> { Users.User1,Users.User5 });

            var users = await userService.GetAllAsync();
            var t = 5;

            Assert.True(users.Count == 2);
        }




    }
}
