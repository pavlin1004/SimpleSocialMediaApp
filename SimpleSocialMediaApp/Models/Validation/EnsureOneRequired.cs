using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using static NuGet.Client.ManagedCodeConventions;

namespace SimpleSocialApp.Models.Validation
{
    public class EnsureOneRequired : ValidationAttribute
    {
        private readonly string[] _properties;

        public EnsureOneRequired(params string[] properties)
        {
            _properties = properties;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            var properties = context.ObjectType.GetProperties();

            bool isAnyPropertyValid = _properties.Any(propertyName =>
            {
                
                var property = properties.FirstOrDefault(p => p.Name == propertyName);
                if (property != null)
                {
                    
                    var propertyValue = property.GetValue(context.ObjectInstance);
                    return isPropertyValid(propertyValue); 
                }
                return false; 
            });

            if(!isAnyPropertyValid)
            {
                return new ValidationResult("At least one element should be selected for publishing!");   
            }
            return ValidationResult.Success;
        }

        private bool isPropertyValid(object? value)
        {
            if (value is string str)
            {
                return !String.IsNullOrEmpty(str);
            }
            else if (value is ICollection<IFormFile> collection)
            {
                return collection!=null && collection.Count > 0;
            }
            else if (value == null) return false;
            return true;
        }
    }
}
