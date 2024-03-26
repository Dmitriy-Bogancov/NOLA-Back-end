using NOLA_API.DataModels;

namespace NOLA_API.Domain
{
    public class AdVisitor
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid AdvertisementId { get; set; }
        public Advertisement Post { get; set; }
        public bool IsOwner { get; set; }
    }
}
