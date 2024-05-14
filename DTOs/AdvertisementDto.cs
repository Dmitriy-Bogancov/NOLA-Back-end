

using NOLA_API.DataModels;

namespace NOLA_API.DTOs
{
    public class AdvertisementDto
    {
          public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string[] Banners { get; set; }
        public ICollection<Profile> Visitors { get; set; } = new List<Profile>();

        public List<AdLink>? Links { get; set; } = new List<AdLink>();
    }
}