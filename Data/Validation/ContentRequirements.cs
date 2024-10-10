using NuGet.Protocol;
using SimpleSocialApp.Data.Models;
using System.ComponentModel.DataAnnotations;
#nullable disable warnings

namespace SimpleSocialApp.Data.Validation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ContentOrMediaRequired : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is Message message)
            {
                return Check(message.Content, message.Media);
            }
            else if (value is Comment comment)
            {
                return Check(comment.Content, comment.Media);
            }
            return new ValidationResult("Invalid object type.");
        }

        private ValidationResult Check(string content, ICollection<Media> media)
        {
            if (string.IsNullOrWhiteSpace(content) && (media == null || media.Count == 0))
            {
                return new ValidationResult("Either Content or Media must be provided.");
            }
            return ValidationResult.Success;
        }
    }
}
