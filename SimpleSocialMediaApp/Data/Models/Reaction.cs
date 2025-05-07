using SimpleSocialApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Data.Models
{
    public class Reaction
    {
        public Reaction()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        [Required]
        public string Id { get; set; }

        public ReactType ReactType { get; set; }
   
        public string? CommentId { get; set; }
       
        public string? PostId  { get; set; }  

        public required string UserId { get; set; }

        public virtual required AppUser User { get; set; }

        public virtual Post? Post { get; set; }

        public virtual Comment? Comment { get; set; }

    }
}
