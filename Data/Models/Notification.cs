using SimpleSocialApp.Data.Enums;
using System.Diagnostics;
#pragma warning disable CS8618 

namespace SimpleSocialApp.Data.Models
{
    public class Notification
    {
        public Notification()
        {
            this.Id = Guid.NewGuid().ToString();
        } 
      
        public string Id { get; set; }

        public string Text {  get; set; }

        public DateTime CreatedDateTime { get; set; }

        public NotificationType Type { get; set; }

        public AppUser UserTo { get; set; }

        public AppUser? UserFrom { get; set; }

        public string UserToId {  get; set; }

        public string? UserFromId { get; set; }




    }
}
