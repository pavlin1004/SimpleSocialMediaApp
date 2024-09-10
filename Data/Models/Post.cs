namespace SimpleSocialApp.Data.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string PictureUrl { get; set; } 
        public string VideoUrl { get; set; }  
        public DateTime PostedOn { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<React> Reacts; 

        public ICollection<Comment> Comments;

    }
}
