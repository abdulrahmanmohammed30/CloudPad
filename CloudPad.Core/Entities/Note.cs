namespace CloudPad.Core.Entities
{
    public class Note
    {
        public int NoteId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid NoteGuid { get; set; } 
        public int UserId { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public int? CategoryId { get; set; }

        public bool IsFavorite { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public bool IsPinned { get; set; } = false;

        public bool IsArchived { get; set; } = false;
        public List<Resource> Resources { get; set; } = new List<Resource>();
        public Category? Category { get; set; }
    }
}