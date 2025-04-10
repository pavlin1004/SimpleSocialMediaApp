using SimpleSocialApp.Data.Models;

namespace SimpleSociaMedialApp.Tests.Data
{
    public static class Friendships
    {
        public static Friendship Friendship1 = new Friendship
        {
            Sender = Users.User1,
            Receiver = Users.User2
        };
        public static Friendship Friendship2 = new Friendship
        {
            Sender = Users.User1,
            Receiver = Users.User3
        };
        public static Friendship Friendship3 = new Friendship
        {
            Sender = Users.User1,
            Receiver = Users.User4
        };
        public static Friendship Friendship4 = new Friendship
        {
            Sender = Users.User2,
            Receiver = Users.User3
        };
        public static Friendship Friendship5 = new Friendship
        {
            Sender = Users.User3,
            Receiver = Users.User5
        };
    }
}
