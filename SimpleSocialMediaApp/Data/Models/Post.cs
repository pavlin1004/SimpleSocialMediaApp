using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Data.Models
{
    public class Post{
        public Post()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Media = new HashSet<Media>();
            this.Reacts = new HashSet<Reaction>();
            this.Comments = new HashSet<Comment>();
        }

        [Key]
        public string Id { get; set; }

        [MaxLength(1000)]
        public string? Content { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; }

        public required string UserId { get; set; }

        public virtual AppUser? User { get; set; }

        public virtual ICollection<Media>? Media { get; set; }

        public virtual ICollection<Reaction>? Reacts { get; set; } 

        public virtual ICollection<Comment>? Comments { get; set; }
    }
}
