using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Data.Factory
{
    public static class FriendshipFactory
    {
        public static Friendship CreatePending(AppUser user1, AppUser user2)
        {
            return new Friendship
            {
                Sender = user1,
                Receiver = user2,
                Type = FriendshipType.Pending
            };
        }

        public static Friendship CreateAccepted(AppUser user1, AppUser user2)
        {
            return new Friendship
            {
                Sender = user1,
                Receiver = user2,
                Type = FriendshipType.Accepted
            };
        }
    }
}
