namespace NoteTakingApp.Core.Entities
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Note> Notes { get; set; } = new List<Note>();
        public int UserId { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}