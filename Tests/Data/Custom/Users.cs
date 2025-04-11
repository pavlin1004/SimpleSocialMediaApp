using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Identity.Client;
using SimpleSocialApp.Data.Models;

namespace Tests.Data.Custom
{
    public static class Users
    {
        public const string param1 = "testUser1";
        public const string param2 = "testUser2";
        public const string param3 = "testUser3";
        public const string param4 = "Nikolay";
        public const string param5 = "Kostadin";


        public static readonly AppUser User1 = new AppUser
        {
            FirstName = param1,
            LastName = param1,
            Email = param2,
            UserName = $"{param1} {param1}",
            Media = new Media
            {
                PublicId = param1,
                Url = param1,
            },
            Posts = new List<Post>() { new() },
        };
        public static readonly AppUser User2 = new AppUser
        {
            FirstName = param2,
            LastName = param2,
            Email = param2,
            UserName = $"{param2} {param2}",
            Media = new Media
            {
                PublicId = param2,
                Url = param2,
            }
        };
        public static readonly AppUser User3 = new AppUser
        {
            FirstName = param3,
            LastName = param3,
            Email = param3,
            UserName = $"{param3} {param3}"
        };

        public static readonly AppUser User4 = new AppUser
        {
            FirstName = param4,
            LastName = param4,
            Email = param4,
            UserName = $"{param4} {param4}"
        };
        public static readonly AppUser User5 = new AppUser
        {
            FirstName = param5,
            LastName = param5,
            Email = param5,
            UserName = $"{param5} {param5}"
        };

    }
}
