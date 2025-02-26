using System.ComponentModel.DataAnnotations;

namespace CloudPad.Core.Entities;

public class Country
{
    [Key]
    public short CountryId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string EnglishName { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string ArabicName { get; set; }
    
    [Required]
    [MaxLength(2)]
    public string Alpha2Code { get; set; }
    
    [Required]
    [MaxLength(3)]
    public string Alpha3Code { get; set; }
    
    [Required]
    public short PhoneCode { get; set; }
}