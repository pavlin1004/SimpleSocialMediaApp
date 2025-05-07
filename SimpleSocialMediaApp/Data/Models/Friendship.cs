using SimpleSocialApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Data.Models
{
    public class Friendship
    {
        public Friendship()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        public required FriendshipType Type { get; set; }

        public required DateTime CreatedAt { get; set; }

        public string? SenderId { get; set; }

        public string? ReceiverId { get; set; }
   
        public virtual AppUser? Sender { get; set; }
  
        public virtual AppUser? Receiver { get; set; }
    }
}
