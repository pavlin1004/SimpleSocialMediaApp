﻿using SimpleSocialApp.Data.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleSocialApp.Data.Models
{
    [ContentOrMediaRequired]
    public class Message
    {
        #pragma warning disable CS8618
        public Message()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Media = new HashSet<Media>();
        }
        [Key]
        public string Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedDateTime { get; set;}

        public string UserId { get; set; }

        public virtual AppUser User { get; set; }
        public string ChatId { get; set; }
        public virtual Chat Chat { get; set; }
        public virtual ICollection<Media> Media { get; set; }

        
    }
}
