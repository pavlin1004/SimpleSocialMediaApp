namespace SimpleSocialApp.Data.Models
{
    public class User
    {
        public string Id;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Bio { get; set; }
        public DateTime CreatedOn { get; set; }

        
        public virtual ICollection<Conversation> Conversations { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

    }
}
