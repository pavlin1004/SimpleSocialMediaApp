using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.InputModels.Chat;

namespace Tests.Data.Factory
{
    public static class ChatFactory
    {
        public static Chat CreateSingle(List<AppUser> users, ChatType type = ChatType.Group)
        {
            return new Chat
            {
                Users = users,
                Type = type
            };
        }

        public static List<Chat> CreateList(List<AppUser> users,int count, ChatType type = ChatType.Private)
        {
            var chatList = new List<Chat>();
            for(int i=0;i<count;i++)
            {
                chatList.Add(
                new Chat
                {
                    Users = users,
                    Type = type
                });
            }
            return chatList;
        }
    }
}
