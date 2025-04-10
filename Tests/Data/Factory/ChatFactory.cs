using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.InputModels.Chat;

namespace Tests.Data.Factory
{
    public static class ChatFactory
    {
        public static Chat CreateChat(List<AppUser> users, ChatType type)
        {
            return new Chat
            {
                Users = users,
                Type = type
            };
        }
    }
}
