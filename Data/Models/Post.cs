namespace SimpleSocialApp.Data.Models
{
    public class Post
    {
        public Post()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Text { get; set; }
        public string PictureUrl { get; set; } 
        public string VideoUrl { get; set; }  
        public DateTime PostedOn { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public ICollection<Reaction> Reacts; 

        public ICollection<Comment> Comments;

    }
}
