using NOLA_API.Domain;

namespace NOLA_API.DataModels
{
    public class Advertisement
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Banner { get; set; }
        public DateTime CreatedAt { get; set; }
        public Status Status { get; set; } 
        public ICollection<AdVisitor> Visitors { get ; set; } = new List<AdVisitor>();
    }
}

public enum Status
{
    Active,
    Acrcived,
    Deleted
}