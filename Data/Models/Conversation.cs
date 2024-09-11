using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Data.Models
{
    public class Conversation
    {
#pragma warning disable CS8618
        public Conversation()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Messages = new HashSet<Message>();
            this.Friends = new HashSet<AppUser>();
        }
        [Key]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }

        public virtual ICollection<Message> Messages { get; set;}

        public virtual ICollection<AppUser> Friends  { get; set;}
    }
}
