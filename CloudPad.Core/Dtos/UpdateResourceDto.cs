using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.Core.Dtos;

public class UpdateResourceDto
{
    [ReadOnly(true)]
    public Guid ResourceId { get; set; }

    [MaxLength(255)]
    [DisplayName("Display Name")]
    public string? DisplayName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [ReadOnly(true)]
    public Guid NoteId { get; set; }

    [ReadOnly(true)]
    public string FilePath { get; set; } = string.Empty;

    [ReadOnly(true)]
    public long? Size { get; set; }
}