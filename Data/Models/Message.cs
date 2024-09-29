using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleSocialApp.Data.Models
{
    public class Message
    {
#pragma warning disable CS8618
        public Message()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string ConversationId { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime TimeSent { get; set; }

        public virtual AppUser Sender { get; set; }
    }
}
