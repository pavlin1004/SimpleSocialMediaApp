using SimpleSocialApp.Data.Enums;

namespace SimpleSocialApp.Data.Models
{
    public class Reaction
    {
      public Reaction()
      {
            this.Id = Guid.NewGuid().ToString();
      }
      public string Id { get; set; }

      public string CommentId { get; set; }
      public string PostId  { get; set; }

      public string UserId { get; set; }

    }
}
