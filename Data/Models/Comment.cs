namespace SimpleSocialApp.Data.Models
{
    public class Comment
    {

        public int CommentId { get; set; }
        public string PostId { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedOnDate { get; set; }


        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<React> Reacts { get; set; }

        public virtual User User1 { get; set; }


           

    }
}
