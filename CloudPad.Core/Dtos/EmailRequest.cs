

using System.ComponentModel.DataAnnotations;

namespace CloudPad.Core.Dtos
{
    public class EmailRequest
    {
        [Required]
        [EmailAddress]
        public string To { get; set; } = string.Empty;
        
        [Required]
        public string Subject { get; set; } = string.Empty;
        
        [Required]
        public string Body { get; set; } = string.Empty;
        
        public bool IsBodyHtml { get; set; }
    }
}
