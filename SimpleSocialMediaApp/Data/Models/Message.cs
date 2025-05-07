using SimpleSocialApp.Data.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleSocialApp.Data.Models
{
    public class Message
    {
        public Message()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Media = new HashSet<Media>();
        }
        [Key]
        public string Id { get; set; }

        public string? Content { get; set; }

        public DateTime CreatedDateTime { get; set;}

        public required string UserId { get; set; }

        public required string ChatId { get; set; }

        public virtual AppUser? User { get; set; }

        public virtual Chat? Chat { get; set; }

        public virtual ICollection<Media> Media { get; set; }

        
    }
}
