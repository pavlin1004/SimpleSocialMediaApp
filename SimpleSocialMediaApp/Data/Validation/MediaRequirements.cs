using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
#nullable disable warnings
public class OneRequiredAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var properties = new[] { "PostId", "CommentId", "MessageId" };

        var type = validationContext.ObjectType;
        var propertyValues = properties.Select(p => type.GetProperty(p)?.GetValue(validationContext.ObjectInstance)).ToList();

        int nonNullCount = propertyValues.Count(v => v != null && !string.IsNullOrEmpty(v?.ToString()));

        if (nonNullCount == 1)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("Exactly one of PostId, CommentId, or MessageId must be initialized.");
    }
}