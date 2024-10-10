using SimpleSocialApp.Data.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleSocialApp.Data.Models
{
    [ContentOrMediaRequired]
    public class Message
    {
        #pragma warning disable CS8618
        public Message()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Media = new HashSet<Media>();
        }
        [Key]
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime TimeSent { get; set; }  
        public string ConversationId { get; set; }

        public virtual Conversation Conversation { get; set; }
        public virtual ICollection<Media> Media { get; set; }

        
    }
}
