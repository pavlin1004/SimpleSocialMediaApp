using SimpleSocialApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Data.Models
{
    public class Media
    {
        public Media()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        public required string Url { get; set; }

        public required string PublicId { get; set; }

        public required MediaOptions Type { get; set; }

        public string? PostId { get; set; }

        public string? CommentId { get; set; }

        public string? MessageId { get; set; }

        public string? UserId { get; set; }

        public virtual Message? Message { get; set; }

        public virtual AppUser? User { get; set; }

        public virtual Post? Post { get; set; }

        public virtual Comment? Comment { get; set; }

    }
}
