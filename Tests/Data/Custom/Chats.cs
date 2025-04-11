using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Data.Custom
{
    public static class Chats
    {

        public static Chat Chat1 = new Chat()
        {
            Type = ChatType.Private,
            Users = {Users.User1, Users.User2}
        };

        public static Chat Chat2 = new Chat()
        {
            Type = ChatType.Private,
            Users = { Users.User1, Users.User3 }
        };

        public static Chat Chat3 = new Chat()
        {
            Type = ChatType.Group,
            Users = { Users.User1, Users.User2, Users.User3 },
            Title = "test"
        };
    }
            
         
}
