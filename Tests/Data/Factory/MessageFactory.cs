using SimpleSocialApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Data.Factory
{
    public static class MessageFactory
    {
        private static DateTime GetRandomDate(DateTime start, DateTime end)
        {
            var rand = new Random();
            var range = (end - start).Days;
            return start.AddDays(rand.Next(range)).AddHours(rand.Next(0, 24)).AddMinutes(rand.Next(0, 60));
        }
        public static List<Message> CreateList(AppUser user, int count)
        {
            var messageList = new List<Message>();
            for(int i=0;i<count; i++)
            {
                messageList.Add(new Message
                {
                    Content= "randomContent",
                    User = user,
                    CreatedDateTime = GetRandomDate(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow)
                });
            }
            return messageList;
        }
    }
}
