using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Services.Implementations;
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

namespace Tests.ServiceTests
{
    public class ChatServiceTests : Initialise
    {
        private readonly ChatService chatService;

        public ChatServiceTests()
        {
            chatService = new ChatService(context);
        }

        [Fact]
        public async Task ShouldGetChatById()
        {
            var chat = ChatFactory.CreateChat(AppUserFactory.CreateUsers(3), ChatType.Private);
            await context.SeedAsync(new List<Chat> { chat });

            var testResult = await chatService.GetChatAsync(chat.Id);

            Assert.NotNull(testResult);
            Assert.Equal(chat.Id, testResult.Id);
        }

        [Fact]

        public async Task ShouldReturnListOfchatsForUserById()
        {
            var users = AppUserFactory.CreateUsers(3);
            var firstChat = ChatFactory.CreateChat(new List<AppUser> { users[0], users[1] });
            var secondChat = ChatFactory.CreateChat(new List<AppUser> { users[0], users[2] });

            await context.SeedAsync(new List<Chat> { firstChat, secondChat });

            var firstResult = await chatService.GetChatsForUserAsync(users[0].Id);
            var secondResult = await chatService.GetChatsForUserAsync(users[1].Id);

            Assert.NotNull(firstResult);
            Assert.NotNull(secondResult);
            Assert.True(firstResult.Count == 2);
            Assert.True(secondResult.Count == 1);
        }

       [Fact]
       public async Task ShouldCreateChat()
        {
            var chat = ChatFactory.CreateChat(AppUserFactory.CreateUsers(2));

            await chatService.CreateChatAsync(chat);

            var result = await chatService.GetChatAsync(chat.Id);

            Assert.NotNull(result);
            Assert.Equal(chat.Id, result.Id);
        }

        [Fact]
        public async Task ShouldAddUserToChat()
        {
            var users = AppUserFactory.CreateUsers(2);
            var chats = new List<Chat> { ChatFactory.CreateChat(users) };
            await context.SeedAsync(users).SeedAsync(chats);

            var user = AppUserFactory.CreateAsync();
            await chatService.AddUserAsync(chats[0],user);

            var result = await chatService.GetChatAsync(chats[0].Id);

            Assert.NotNull(result);
            Assert.True(result.Users.Count == 3);
        }

        [Fact]
        public async Task ShouldRemoveUserFromChatAsync()
        {
            var users = AppUserFactory.CreateUsers(2);
            var chats = new List<Chat> { ChatFactory.CreateChat(users) };
            await context.SeedAsync(users).SeedAsync(chats);

            await chatService.RemoveUserAsync(chats[0], users[1]);

            var result = await chatService.GetChatAsync(chats[0].Id);

            Assert.NotNull(result);
            Assert.True(result.Users.Count == 1);
        }

        [Fact]
        public async Task ShouldTakeAllUsersFromChatsAndReturnList()
        {
            var users = AppUserFactory.CreateUsers(2);
            var chats = new List<Chat> { ChatFactory.CreateChat(users) };
            await context.SeedAsync(users).SeedAsync(chats);

            var result = await chatService.GetAllUsersAsync(chats[0]);
            Assert.True(result.Count == 2);
            Assert.IsType<List<AppUser>>(result);
        }

        [Fact]
        public async Task ShouldGetAllChatsBasedOnSearchQuery()
        {
            var currentUser = Users.User1;
            await context.SeedAsync(new List<Chat> { Chats.Chat1, Chats.Chat2, Chats.Chat3 });

            const string searchQuery1 = "test";
            const string searchQuery2 = "user";

            var result1 = await chatService.SearchChatAsync(currentUser.Id, searchQuery1);
            var result2 = await chatService.SearchChatAsync(currentUser.Id, searchQuery2);

            Assert.True(result1.Count == 3);
            Assert.True(result2.Count == 2);

        }

        [Fact]
        public async Task ShouldGetUserChatsSortedByLastMessage()
        {
            var users = AppUserFactory.CreateUsers(1);// return list except one entity
            var chats = ChatFactory.CreateAsync(users, 3);

            foreach (var chat in chats)
            {
                chat.Messages = MessageFactory.CreateAsync(users[0], 3);
            }

            await context.SeedAsync(chats);

            var orderedChats = await chatService.OrderChatsByLastMessages(users[0].Id, 3);// should get top 3 chats for user

            var lastMessageDates = orderedChats 
            .Select(c => c.Messages?.Max(m => m.CreatedDateTime))
            .ToList();

            Assert.True(lastMessageDates[0] >= lastMessageDates[1]);
            Assert.True(lastMessageDates[1] >= lastMessageDates[2]);
        }

        [Fact]
        public async Task ShouldCreatePrivateChatIfUserNotNull()
        {
            var users = AppUserFactory.CreateUsers(2);

            await context.SeedAsync(users);

            await chatService.CreatePrivateChatAsync(users[0], users[1]);

            var chats = await chatService.GetChatsForUserAsync(users[0].Id);

            Assert.True(chats.Count == 1);
        }

        [Fact]
        public async Task ShouldNotCreateAnyChatIfAtLeastOneUserIsNull()
        {
            var users = AppUserFactory.CreateUsers(1);

            await context.SeedAsync(users);

            await chatService.CreatePrivateChatAsync(users[0], null);
            await chatService.CreatePrivateChatAsync(null, users[0]);
            await chatService.CreatePrivateChatAsync(null, null);

            Assert.True(context.Chats.Count() == 0);
        }
    }
}
