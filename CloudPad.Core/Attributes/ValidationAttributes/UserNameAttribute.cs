using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace NoteTakingApp.Core.Attributes.ValidationAttributes;

public class UserNameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return null;

        var userName = ((string)value).Trim();
        
        if (userName.Length is < 6 or > 20)
            return new ValidationResult("Username must be between 6 and 20 characters");
        
        /*if (userName.Contains(' '))
            return new ValidationResult("Username cannot contain spaces between characters");
        
        if (!char.IsAsciiLetter(userName.ElementAt(0)))
            return new ValidationResult("Username must start with a character");
        
        if (!Regex.IsMatch(userName, @"^[a-zA-Z0-9]+$"))
            return new ValidationResult("Username must contain only letters and digits.");*/
        
        return ValidationResult.Success;
    }
}