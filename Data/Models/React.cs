using SimpleSocialApp.Data.Enums;

namespace SimpleSocialApp.Data.Models
{
    public class React
    {
      public string ReactId { get; set; }

      public string CommentId { get; set; }
      public string PostId  { get; set; }

      public string UserId { get; set; }

      public virtual User User { get; set; }
      public virtual Comment Comment { get; set; }
      public virtual Post Post { get; set; }

    }
}
