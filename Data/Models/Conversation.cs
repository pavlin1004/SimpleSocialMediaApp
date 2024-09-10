namespace SimpleSocialApp.Data.Models
{
    public class Conversation
    {

        public string ConversationId { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<Message> Messages { get; set;}

        public virtual ICollection<User> Users  { get; set;}
    }
}
