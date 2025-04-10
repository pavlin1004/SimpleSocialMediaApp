using SimpleSocialApp.Data.Models;

namespace SimpleSociaMedialApp.Tests.Data
{
    public static class Chats
    {

        public static readonly Chat Chat1 = new Chat
        {
            Users =
            {
                Users.User1,
                Users.User2,
                Users.User3,
                Users.User4,
            },
            Type = SimpleSocialApp.Data.Enums.ChatType.Group
        };
        public static readonly Chat Chat2 = new Chat
        {
            Users =
            {
                Users.User1,
                Users.User2,
            },
            Type = SimpleSocialApp.Data.Enums.ChatType.Private
        };
    }
}
