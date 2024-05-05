using NOLA_API.Domain;

namespace NOLA_API.DataModels
{
    public class Draft
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string[] Banners { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
