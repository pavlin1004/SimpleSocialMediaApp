namespace SimpleSocialApp.Data.Models
{
    public class User
    {
        Guid id;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public DateTime createOn;

        public string Bio;

        public ICollection<Friendship> friends;

    }
}
