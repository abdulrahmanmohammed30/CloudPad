namespace NoteTakingApp.Core.Attributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class PasswordComplexityAttribute : ValidationAttribute
{
    private readonly int _minLength;
    
    public PasswordComplexityAttribute(int minLength = 8)
    {
        _minLength = minLength;
        ErrorMessage = $"Password must be at least {_minLength} characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character.";
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
            return null;

        string password = value.ToString();

        if (password.Length < _minLength)
            return new ValidationResult($"Password must be at least {_minLength} characters long.");

        if (!Regex.IsMatch(password, @"[A-Z]")) 
            return new ValidationResult("Password must contain at least one uppercase letter.");

        if (!Regex.IsMatch(password, @"[a-z]")) 
            return new ValidationResult("Password must contain at least one lowercase letter.");

        if (!Regex.IsMatch(password, @"\d")) 
            return new ValidationResult("Password must contain at least one digit.");

        if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{}|;':"",.<>?/\\]")) 
            return new ValidationResult("Password must contain at least one special character.");

        return ValidationResult.Success;
    }
}
