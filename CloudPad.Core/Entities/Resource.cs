namespace NoteTakingApp.Core.Entities
{
    public class Resource
    {
        public Guid ResourceId { get; set; } 
        public string FilePath { get; set; } = string.Empty;

        public string? DisplayName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public long? Size { get; set; }

        public int NoteId { get; set; }

        public Note Note { get; set; } = new Note();

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}
