namespace SimpleSocialApp.Data.Models
{
    public class Friendship
    {
        public int Id { get; set; }

        public string User1Id { get; set; } // First user in the friendship
        public User User1 { get; set; }

        public string User2Id { get; set; } // Second user in the friendship
        public User User2 { get; set; }

        public bool IsAccepted { get; set; } // Whether the friend request is accepted
        public DateTime CreatedOn { get; set; }
        
    }
}
