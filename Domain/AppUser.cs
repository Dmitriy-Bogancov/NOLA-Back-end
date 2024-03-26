using Microsoft.AspNetCore.Identity;

namespace NOLA_API.Domain
{
    public class AppUser : IdentityUser
    {
        public ICollection<AdVisitor> Posts { get; set; }
    }
}