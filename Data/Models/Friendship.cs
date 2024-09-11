using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SimpleSocialApp.Data.Models
{
    public class Friendship
    {
#pragma warning disable CS8618
        public Friendship()

        {
            this.Id = Guid.NewGuid().ToString();
        }


        [Key]
        public string Id { get; set; }

        [Required]
        public string User1Id { get; set; }

        [Required]
        public string User2Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
   
        public virtual AppUser User { get; set; }
  
        public virtual AppUser Friend { get; set; }
    }
}
