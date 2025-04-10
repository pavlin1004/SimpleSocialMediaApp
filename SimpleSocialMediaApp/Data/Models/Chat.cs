using SimpleSocialApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Data.Models
{
    public class Chat
    {
        #pragma warning disable CS8618
        public Chat()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Messages = new HashSet<Message>();
            this.Users = new HashSet<AppUser>();
        }
        [Key]
        public string Id { get; set; }
        public string? Title { get; set; }
        public DateTime CreatedDateTime { get; set; }
        bool IsDeleted { get; set; } // for keeping two users chat in the database except deleting the whole correspondation
        public ChatType Type { get; set; } // group or private (2 friends) chat

        public string? OwnerId { get; set; }
        public virtual ICollection<Message>? Messages { get; set;}

        public virtual ICollection<AppUser> Users  { get; set;}
    }
}
