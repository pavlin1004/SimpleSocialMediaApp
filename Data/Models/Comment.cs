using SimpleSocialApp.Data.Validation;
using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618 

namespace SimpleSocialApp.Data.Models
{
    [ContentOrMediaRequired]
    public class Comment
    {
        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
           this.Comments = new HashSet<Comment>();
            this.Reacts = new HashSet<Reaction>();
        }
        [Key]
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public string UserId { get; set; }
        public string? PostId { get; set; }
        public string? ParentCommentId { get; set; }

        public virtual Comment ParentComment { get; set; }

        public virtual Post Post { get; set; }

        public virtual AppUser User { get; set; }

        public virtual ICollection<Media> Media { get; set; } 

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Reaction> Reacts { get; set; }
 
    }
}
