namespace CloudPad.Core.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid CategoryGuid { get; set; } 

        public List<Note> Notes { get; set; } = new List<Note>();

        public bool IsFavorite { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public int UserId { get; set; }

    }
}