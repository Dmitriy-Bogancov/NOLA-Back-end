using NOLA_API.Domain;

namespace NOLA_API.DataModels
{
    public class Advertisement
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<AdVisitor> Visitors { get ; set; } = new List<AdVisitor>();
    }
}