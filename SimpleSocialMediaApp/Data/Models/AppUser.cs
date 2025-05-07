using Microsoft.AspNetCore.Identity;
using SimpleSocialApp.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SimpleSocialApp.Data.Models
{
    public class AppUser : IdentityUser 
    {        
        public AppUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Friendships = new HashSet<Friendship>();
            this.Chats = new HashSet<Chat>();
            this.Posts = new HashSet<Post>();
            this.Notifications = new HashSet<Notification>();
        }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(40, ErrorMessage = "Last Name can't be longer than 40 characters")]
        public required string FirstName { get; set; }


        [Required(ErrorMessage = "Last name is required")]
        [StringLength(40, ErrorMessage = "Last Name can't be longer than 40 characters")]
        public required string LastName { get; set; }

        public GenderType Gender { get; set; }

        public string? MediaId { get; set; } // profile pic

        public virtual Media? Media { get; set; }

        public virtual ICollection<Friendship> Friendships { get; set; }

        public virtual ICollection<Chat> Chats { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
