using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Data.Models
{
    public class AppUser : IdentityUser
    {
#pragma warning disable CS8618
        public AppUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Friendships = new HashSet<Friendship>();
            this.Conversations = new HashSet<Conversation>();
            this.Posts = new HashSet<Post>();
        }
        [Required(ErrorMessage = "First name is required")]
        [StringLength(40, ErrorMessage = "Last Name can't be longer than 40 characters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(40, ErrorMessage = "Last Name can't be longer than 40 characters")]
        public string LastName { get; set; }

        public string? MediaId { get; set; }
        public virtual Media? Media { get; set; }
        public virtual ICollection<Friendship> Friendships { get; set; }
        public virtual ICollection<Conversation> Conversations { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
