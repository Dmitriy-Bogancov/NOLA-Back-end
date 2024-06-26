using NOLA_API.DataModels;
using NOLA_API.Domain;

namespace NOLA_API.DataModels
{
    public class Advertisement
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string[] Banners { get; set; }
        public DateTime CreatedAt { get; set; }
        public Status Status { get; set; }
        public ICollection<AdVisitor> Visitors { get; set; } = new List<AdVisitor>();

        public List<AdLink> Links { get; set; } = new List<AdLink>();
    }
}

public class AdLink
{
    public Guid AdvertisementId { get; set; }    
    public Guid Id { get; set; }
    public Advertisement? Advertisement { get; set; }
    public string Action { get; set; }
    public string Href { get; set; }
}

public enum Status
{
    Moderation,
    Active,
    Archived,
    Deleted
}