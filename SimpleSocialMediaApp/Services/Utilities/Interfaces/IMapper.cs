﻿using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.ViewModels.AppUsers;
using SimpleSocialApp.Models.ViewModels.Chats;

namespace SimpleSociaMedialApp.Services.Utilities.Interfaces
{
    public interface IMapper
    {
        public ChatViewModel MapToChatViewModel(Chat chat, AppUser? friend, int count, int size);
        public ChatViewModel MapToChatViewModel(Chat chat, int count, int size);
        public List<UserViewModel> MapToUserViewModel(List<AppUser> pending, List<AppUser> nonFriends);
        public FriendViewModel MapToFriendsViewModel(List<AppUser> friends, string userId);
        public UserViewModel MapToUserViewModel(AppUser user, Friendship? friendship);
    }
}
