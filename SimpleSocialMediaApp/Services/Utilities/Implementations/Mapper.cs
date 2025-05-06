using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.ViewModels.AppUsers;
using SimpleSocialApp.Models.ViewModels.Chats;
using SimpleSocialApp.Models.ViewModels.Posts;
using SimpleSociaMedialApp.Services.Utilities.Interfaces;

namespace SimpleSociaMedialApp.Services.Utilities.Implementations
{
    public class Mapper : IMapper
    {
        public ChatViewModel MapToChatViewModel(Chat chat, AppUser? friend, int count, int size)
        {
            return new ChatViewModel
            {
                ChatId = chat.Id,
                Messages = chat.Messages?.OrderByDescending(c => c.CreatedDateTime).Skip(size * count)
                .Take(size).ToList() ?? new List<Message>(),
                Type = chat.Type,
                Friend = friend,
                Title = null,
                OwnerId = null
            };
        }
        public ChatViewModel MapToChatViewModel(Chat chat, int count, int size)
        {
            return new ChatViewModel
            {
                ChatId = chat.Id,
                Messages = chat.Messages?.OrderByDescending(c => c.CreatedDateTime).Skip(size * count)
                .Take(size).ToList() ?? new List<Message>(),
                Type = chat.Type,
                Friend = null,
                Title = chat.Title,
                OwnerId = chat.OwnerId
            };
        }

        public List<UserViewModel> MapToUserViewModel(List<AppUser> pending, List<AppUser> nonFriends)
        {
            var pendingUser = pending.Select(u => new UserViewModel
            {
                User = u,
                Type = FriendshipType.Pending
            }).ToList();

            var nonFriendUsers = nonFriends.Select(u => new UserViewModel
            {
                User = u,
                Type = null
            }).ToList();

            return pendingUser.Concat(nonFriendUsers).ToList();
        }
        public FriendViewModel MapToFriendsViewModel(List<AppUser> friends, string userId)
        {
            return new FriendViewModel
            {
                Friends = friends.Select(user => new UserViewModel
                {
                    User = user,
                    Type = FriendshipType.Accepted
                }).ToList(),
                UserId = userId
            };
        }
        public UserViewModel MapToUserViewModel(AppUser user, Friendship? friendship)
        {
            return new UserViewModel
            {
                User = user,
                Type = friendship != null ? friendship.Type : null
            };
        }
    }
}
