using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using NoteTakingApp.Core.Models;

namespace NoteTakingApp.Core.Domains;

public class ApplicationUser:IdentityUser<int>
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }=string.Empty;
    
    [MaxLength(500)]
    public string? ProfileImageUrl { get; set; }

    [ForeignKey(nameof(Country))]
    public short? CountryId { get; set; }
    public Country? Country { get; set; }
    
    public DateOnly? BirthDate { get; set; }
}