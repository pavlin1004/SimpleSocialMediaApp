using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Data.Models
{
    public class Comment
    {
#pragma warning disable CS8618
        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Comments = new HashSet<Comment>();
            this.Reacts = new HashSet<Reaction>();
        }
        [Key]
        public string Id { get; set; }

        public string PostId { get; set; } // or ParentCommentId
        [Required]
        public string UserId { get; set; }
        [Required]
        public DateTime CreatedOnDate { get; set; }
        
        public string ParentCommentId { get; set; }

        public virtual Comment ParentComment { get; set; }

        public virtual AppUser User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Reaction> Reacts { get; set; }

        
   
    }
}
