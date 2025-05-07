using SimpleSocialApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Data.Models
{
    public class Notification
    {
        public Notification()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        public required string Text {  get; set; }

        public required DateTime CreatedDateTime { get; set; }

        public required NotificationType Type { get; set; }

        public string? UserToId { get; set; }

        public string? UserFromId { get; set; }

        public virtual AppUser? UserTo { get; set; }

        public virtual AppUser? UserFrom { get; set; }





    }
}
