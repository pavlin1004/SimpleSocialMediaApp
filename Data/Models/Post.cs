using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Data.Models
{
    public class Post
    {
#pragma warning disable CS8618
        public Post()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Media = new HashSet<Media>();
            this.Reacts = new HashSet<Reaction>(); 
            this.Comments = new HashSet<Comment>();
        }
        [Key]
        public string Id { get; set; }
     

        [StringLength(40, ErrorMessage = "Last Name can't be longer than 40 characters")]
        public string Text { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }

        public string UserId { get; set; }

        public virtual AppUser User { get; set; }

        public virtual ICollection<Media> Media { get; set; }

        public virtual ICollection<Reaction> Reacts { get; set; } 

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
