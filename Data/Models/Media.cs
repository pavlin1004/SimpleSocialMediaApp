using SimpleSocialApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Data.Models
{
    public class Media
    {
#pragma warning disable CS8618
        public Media()
        {
            this.Id = Guid.NewGuid().ToString();
        }



        [Key]
        public string Id { get; set; }

        [Required]
        public string Url { get; set; }

        public string PublicId { get; set; }
        public MediaOptions Type { get; set; }
        public string? PostId { get; set; }
        public string? CommentId { get; set; }
        public string? MessageId { get; set; }
        public string? UserId { get; set; }

        public virtual Message Message { get; set; }
        public virtual AppUser User { get; set; }
        public virtual Post Post { get; set; }
        public virtual Comment Comment { get; set; }

    }
}
