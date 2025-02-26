using System.ComponentModel.DataAnnotations;

namespace CloudPad.Core.Attributes.ValidationAttributes;

public class BirthDateValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return null; 
        
        if (value is not DateOnly dateOnly)
            return new ValidationResult("Date must be of type DateOnly");
        
        if (dateOnly >= DateOnly.FromDateTime(DateTime.Now))
            return new ValidationResult("Invalid date.");

        return ValidationResult.Success;
    }
}