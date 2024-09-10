namespace SimpleSocialApp.Data.Models
{
    public class Conversation
    {
        public Conversation()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }

        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual ICollection<Message> Messages { get; set;}

        public virtual ICollection<User> Friends  { get; set;}
    }
}
