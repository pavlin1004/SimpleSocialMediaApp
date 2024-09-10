namespace SimpleSocialApp.Data.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string PostId { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public string ParentCommentId { get; set; }

        public virtual Comment ParentComment { get; set; } 

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Reaction> Reacts { get; set; }

        public virtual User User { get; set; }
   
    }
}
