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
            this.Users = new HashSet<AppUser>();
        }
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title can't be longer than 100 characters.")]
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual ICollection<Message> Messages { get; set;}

        public virtual ICollection<AppUser> Users  { get; set;}
    }
}
