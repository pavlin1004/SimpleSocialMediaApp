using Microsoft.Identity.Client;
using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Migrations;
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
    public class FriendsipServiceTests : TestBase
    {
        private readonly FriendshipsService service;

        public FriendsipServiceTests()
        {
            service = new FriendshipsService(context);
        }

        [Fact]
        public async Task ShouldGetFriendshipForTwoUsers()
        {
            var users = AppUserFactory.CreateList(4);
            var friendship1 = FriendshipFactory.CreateAccepted(users[0], users[1]);
            var friendship2 = FriendshipFactory.CreateAccepted(users[0], users[2]);

            await context.SeedAsync(users).SeedEntityAsync(friendship1).SeedEntityAsync(friendship2);

            var result1 = await service.GetFriendshipById(users[0].Id, users[1].Id);
            var result2 = await service.GetFriendshipById(users[0].Id, users[2].Id);
            var result3 = await service.GetFriendshipById(users[1].Id, users[2].Id);
            var result4 = await service.GetFriendshipById(users[2].Id, users[3].Id);

            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Null(result3);
            Assert.Null(result4);
        }

        [Fact]

        public async Task ShouldAcceptPendingFriendshipAsync()
        {
            var users = AppUserFactory.CreateList(2);
            var friendship = FriendshipFactory.CreatePending(users[0], users[1]);

            await context.SeedAsync(new List<Friendship> { friendship });

            await service.AcceptUserFriendshipAsync(users[0].Id, users[1].Id);

            var result = await service.GetUserAcceptedFriendshipsAsync(users[0].Id);

            Assert.NotNull(result);
            Assert.Equal(FriendshipType.Accepted, result[0].Type);
        }

        [Fact] 
        public async Task ShouldRemoveFriendshipFromDatabase()
        {
            var users = AppUserFactory.CreateList(2);
            var friendship = FriendshipFactory.CreatePending(users[0], users[1]);

            await context.SeedAsync(new List<Friendship> { friendship });

            await service.RemoveUserFriendshipsAsync(users[0].Id, users[1].Id);

            Assert.Empty(context.Friendships);
        }

        [Fact]
        public async Task ShouldCreatePendingRequestIfNoFriendshipExists()
        {
            var users = AppUserFactory.CreateList(2);
            await context.SeedAsync(users);

            await service.SendFriendshipRequestAsync(users[0].Id, users[1].Id);

            var friendship = await service.GetFriendshipById(users[0].Id, users[1].Id);

            Assert.NotNull(friendship);
            Assert.Equal(FriendshipType.Pending, friendship.Type);
        }

        [Fact]
        public async Task ShouldNotCreateAnyFriendshipRequestIfAlreadyExists()
        {
            var users = AppUserFactory.CreateList(2);
            await context.SeedAsync(users).SeedAsync(new List<Friendship>() { FriendshipFactory.CreatePending(users[0], users[1]) });

            await service.SendFriendshipRequestAsync(users[0].Id, users[1].Id);

            var friendship = await service.GetFriendshipById(users[0].Id, users[1].Id);

            Assert.NotNull(friendship);
            Assert.Single(context.Friendships);
        }

        [Fact]
        public async Task ShouldCheckFriendshipAndReturnItOrNull()
        {
            var users = AppUserFactory.CreateList(3);
            var friendship = FriendshipFactory.CreateAccepted(users[0], users[1]);

            await context.SeedAsync(users).SeedEntityAsync(friendship);

            var result1 = await service.CheckFriendship(users[0].Id, users[1].Id);
            var result2 = await service.CheckFriendship(users[0].Id, users[2].Id);

            Assert.NotNull(result1);
            Assert.Equal(friendship.Id, result1.Id);
            Assert.Null(result2);   
        }

        [Fact]
        public async Task ShouldGetAllFriendsForUser()
        {
            var users = AppUserFactory.CreateList(4);
            var friendship1 = FriendshipFactory.CreateAccepted(users[0], users[1]);
            var friendship2 = FriendshipFactory.CreateAccepted(users[0], users[2]);

            await context.SeedAsync(users).SeedEntityAsync(friendship1).SeedEntityAsync(friendship2);

            var result = await service.GetAllFriends(users[0].Id);

            Assert.NotNull(result);
            Assert.IsType<List<AppUser>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task ShouldGetAllFriendsIdsForUser()
        {
            var users = AppUserFactory.CreateList(4);
            var friendship1 = FriendshipFactory.CreateAccepted(users[0], users[1]);
            var friendship2 = FriendshipFactory.CreateAccepted(users[0], users[2]);

            await context.SeedAsync(users).SeedEntityAsync(friendship1).SeedEntityAsync(friendship2);

            var result = await service.GetAllFriendsIds(users[0].Id);

            Assert.NotNull(result);
            Assert.IsType<string>(result[0]);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task ShouldCreateFriendship()
        {
            var users = AppUserFactory.CreateList(2);
            var friendship = FriendshipFactory.CreatePending(users[0], users[1]);
            await service.CreateAsync(friendship);
            Assert.Single(context.Friendships);
        }

        [Fact]
        public async Task ShouldGetAllUsersWithNoAcceptedFriendshipStatus()
        {
            var users = AppUserFactory.CreateList(4);
            var friendship1 = FriendshipFactory.CreateAccepted(users[0], users[1]);
            var friendship2 = FriendshipFactory.CreatePending(users[0], users[2]);

            await context.SeedAsync(users).SeedEntityAsync(friendship1).SeedEntityAsync(friendship2);

            var result = await service.GetNonFriendUsers(users[0].Id);

            Assert.NotNull(result.Item1); //pending requests
            Assert.NotNull(result.Item2); // no request records
            Assert.Single(result.Item1);
            Assert.Single(result.Item2);
        }


    }
}
