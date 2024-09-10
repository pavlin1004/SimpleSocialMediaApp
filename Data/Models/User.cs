using Microsoft.AspNetCore.Identity;

namespace SimpleSocialApp.Data.Models
{
    public class User :  IdentityUser
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Friendship> Friendships { get; set; }
        public virtual ICollection<Conversation> Conversations { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

    }
}
