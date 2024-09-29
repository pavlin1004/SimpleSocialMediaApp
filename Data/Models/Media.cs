using SimpleSocialApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Data.Models
{
    public class Media
    {
        #pragma warning disable CS8618
        public Media()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        [Key]
        public string Id { get; set; }
        [Required]
        [StringLength(2048, ErrorMessage = "Url Name can't be longer than 2048 characters")]
        public string Url { get; set; }

        [Required]
        public MediaType Type { get; set; }

        public string PostId { get; set; }
        public virtual Post Post { get; set; }

    }
}
