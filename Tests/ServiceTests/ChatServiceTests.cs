using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSociaMedialApp.Services.Functional.Implementations;
using SimpleSociaMedialApp.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tests.Common;
using Tests.Data.Custom;
using Tests.Data.Factory;
using Xunit.Sdk;

namespace Tests.ServiceTests
{
    public class ChatServiceTests : TestBase
    {
        private readonly ChatService chatService;

        public ChatServiceTests()
        {
            chatService = new ChatService(context);
        }

        [Fact]
        public async Task ShouldGetChatById()
        {
            var chat = ChatFactory.CreateSingle(AppUserFactory.CreateList(3), ChatType.Private);

            await context.SeedAsync([chat]);

            var testResult = await chatService.GetChatAsync(chat.Id);

            Assert.NotNull(testResult);
            Assert.Equal(chat.Id, testResult.Id);
        }

        [Fact]

        public async Task ShouldReturnListOfchatsForUserById()
        {
            var users = AppUserFactory.CreateList(3);
            var firstChat = ChatFactory.CreateSingle([users[0], users[1]]);
            var secondChat = ChatFactory.CreateSingle([users[0], users[2]]);

            await context.SeedAsync([firstChat, secondChat]);

            var firstResult = await chatService.GetChatsForUserAsync(users[0].Id);
            var secondResult = await chatService.GetChatsForUserAsync(users[1].Id);

            Assert.NotNull(firstResult);
            Assert.NotNull(secondResult);
            Assert.Equal(2,firstResult.Count);
            Assert.Single(secondResult);
        }

       [Fact]
       public async Task ShouldCreateChat()
        {
            var chat = ChatFactory.CreateSingle(AppUserFactory.CreateList(2));

            await chatService.CreateChatAsync(chat);

            var result = await chatService.GetChatAsync(chat.Id);

            Assert.NotNull(result);
            Assert.Equal(chat.Id, result.Id);
        }

        [Fact]
        public async Task ShouldAddUserToChat()
        {
            var users = AppUserFactory.CreateList(2);
            List<Chat> chats = [ChatFactory.CreateSingle(users)];
            await context.SeedAsync(users).SeedAsync(chats);

            var user = AppUserFactory.CreateSingle();
            await chatService.AddUserAsync(chats[0],user);

            var result = await chatService.GetChatAsync(chats[0].Id);

            Assert.NotNull(result);
            Assert.Equal(3,result.Users.Count);
        }

        [Fact]
        public async Task ShouldRemoveUserFromChatAsync()
        {
            var users = AppUserFactory.CreateList(2);
            List<Chat> chats = [ChatFactory.CreateSingle(users)];

            await context.SeedAsync(users).SeedAsync(chats);

            await chatService.RemoveUserAsync(chats[0], users[1]);
            var result = await chatService.GetChatAsync(chats[0].Id);

            Assert.NotNull(result);
            Assert.True(result.Users.Count == 1);
        }

        [Fact]
        public async Task ShouldTakeAllUsersFromChatsAndReturnList()
        {
            var users = AppUserFactory.CreateList(2);
            List<Chat> chats = [ChatFactory.CreateSingle(users)];

            await context.SeedAsync(users).SeedAsync(chats);

            var result = await chatService.GetAllUsersAsync(chats[0]);

            Assert.True(result.Count == 2);
            Assert.IsType<List<AppUser>>(result);
        }

        [Fact]
        public async Task ShouldGetAllChatsBasedOnSearchQuery()
        {
            var currentUser = Users.User1;
            const string searchQuery1 = "test", searchQuery2 = "user";
            
            await context.SeedAsync(new List<Chat> { Chats.Chat1, Chats.Chat2, Chats.Chat3 });

            var result1 = await chatService.SearchChatAsync(currentUser.Id, searchQuery1);
            var result2 = await chatService.SearchChatAsync(currentUser.Id, searchQuery2);

            Assert.Equal(3,result1.Count);
            Assert.Equal(2,result2.Count);
        }

        [Fact]
        public async Task ShouldGetUserChatsSortedByLastMessage()
        {
            var user = AppUserFactory.CreateSingle();// return list except one entity
            var chats = ChatFactory.CreateList([user], 3);

            foreach (var chat in chats)
            {
                chat.Messages = MessageFactory.CreateList(user, 3);
            }

            await context.SeedAsync(chats);

            var orderedChats = await chatService.OrderChatsByLastMessages(user.Id, 3);// should get top 3 chats for user

            var lastMessageDates = orderedChats 
            .Select(c => c.Messages?.Max(m => m.CreatedDateTime))
            .ToList();

            Assert.True(lastMessageDates[0] >= lastMessageDates[1]);
            Assert.True(lastMessageDates[1] >= lastMessageDates[2]);
        }

        [Fact]
        public async Task ShouldCreatePrivateChatIfUserNotNull()
        {
            var users = AppUserFactory.CreateList(2);

            await context.SeedAsync(users);

            await chatService.CreatePrivateChatAsync(users[0], users[1]);
            var chats = await chatService.GetChatsForUserAsync(users[0].Id);

            Assert.Single(chats);
        }

        [Fact]
        public async Task ShouldNotCreateAnyChatIfAtLeastOneUserIsNull()
        {
            var user = AppUserFactory.CreateSingle();

            await context.SeedEntityAsync(user);

            await chatService.CreatePrivateChatAsync(user, null);
            await chatService.CreatePrivateChatAsync(null, user);
            await chatService.CreatePrivateChatAsync(null, null);

            Assert.Empty(context.Chats);
        }
    }
}
