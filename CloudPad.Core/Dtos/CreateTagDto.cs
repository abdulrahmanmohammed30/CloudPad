using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NoteTakingApp.Core.Dtos;

public class CreateTagDto
{
    [MaxLength(50)]
    [Remote("ValidateTagName", "Tag", ErrorMessage ="Name is already taken.")]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
}

// Upload File -> returns file link
// The javascript saves the link 
// then the user completes the form and all the data gets send to the server 
// user can add a resource and then by default it gets uploaded to the server 
// then user can drop a resource and upload different resource all in the same form 
// file path is required so don't submit without the file path 

// CreateResource 

// Get All resources by noteid 

// endpoint get single reosurce by id 