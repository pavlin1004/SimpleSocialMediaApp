using Microsoft.Extensions.Diagnostics.HealthChecks;
using SimpleSocialApp.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SimpleSocialApp.Data.Models
{
    public class Friendship
    {
#pragma warning disable CS8618
        public Friendship()

        {
            this.Id = Guid.NewGuid().ToString();
        }


        [Key]
        public string Id { get; set; }

        public FriendshipType Type { get; set; }

        public DateTime CreatedAt { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }
   
        public virtual AppUser Sender { get; set; }
  
        public virtual AppUser Receiver { get; set; }
    }
}
