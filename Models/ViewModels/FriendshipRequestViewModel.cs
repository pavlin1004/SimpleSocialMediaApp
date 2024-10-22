
using Microsoft.AspNetCore.Mvc;
using SimpleSocialApp.Data.Models;
using System.Diagnostics.CodeAnalysis;
using System.Security.Policy;

namespace SimpleSocialApp.Models.ViewModels
{
    public class FriendshipRequestViewModel
    {
        
        public required string FriendshipId { get; set; }
        public required string UserFromName { get; set; }
        public required DateTime DateSent { get; set; }
    }
     
}
