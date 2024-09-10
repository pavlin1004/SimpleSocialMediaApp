using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleSocialApp.Data.Models
{
    public class Message
    {
        public Message()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string ConversationId { get; set; }
        public string Content { get; set; }
        public DateTime TimeSent { get; set; }


        public virtual User Sender { get; set; }
    }
}
