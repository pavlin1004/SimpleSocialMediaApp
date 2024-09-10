using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleSocialApp.Data.Models
{
    public class Message
    {
        public string MessageId { get; set; }
        public string SenderId { get; set; }
        public string ConversationId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }



        public virtual User Sender { get; set; }
        public virtual User Conversation { get; set; }
    }
}
