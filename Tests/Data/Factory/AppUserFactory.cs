using SimpleSocialApp.Data.Enums;
using SimpleSocialApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Data.Factory
{
    public static class AppUserFactory
    {
        public static AppUser CreateAsync()
        {
            var data = Guid.NewGuid().ToString();
            return new AppUser
            {
                Id = data,
                Email = data,
                FirstName = data,
                LastName = data
            };

        }
        public static List<AppUser> CreateUsers(int count)
        {
            var users = new List<AppUser>();
            for (int i = 0; i < count; i++)
            {
                var data = $"{count}";
                var email = $"{count}@example.com";
                users.Add
                (
                    new AppUser
                    {
                        Email = email,
                        FirstName = data,
                        LastName = data
                    }
                );
            }
            return users;
        }
    }
}
