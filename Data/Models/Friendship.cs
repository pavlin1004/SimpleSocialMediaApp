using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SimpleSocialApp.Data.Models
{
    public class Friendship
    {
        public Friendship()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string User1Id { get; set; }  
        public string User2Id { get; set; }  
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; }
        public virtual User Friend { get; set; }
    }
}
