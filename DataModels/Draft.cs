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
        public List<DraftLink> Links { get; set; } = new List<DraftLink>();
    }

    public class DraftLink
    {
        public Guid DraftId { get; set; }
        public Guid Id { get; set; }
        public Draft Draft { get; set; }
        public string Action { get; set; }
        public string Href { get; set; }
    }
}
