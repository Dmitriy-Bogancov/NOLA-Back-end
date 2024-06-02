using Microsoft.AspNetCore.Identity;
using Npgsql;

namespace NOLA_API.Domain
{
    public class AppUser : IdentityUser
    {
        public string? Token { get; set; } = "";
        public string? Image { get; set; } = "";
        public ICollection<AdVisitor>? Posts { get; set; } = new List<AdVisitor>();
        public ICollection<AdVisitor>? Saved { get; set; } = new List<AdVisitor>();
        public List<string>? Links { get; set; } = new List<string>();
        public bool EmailConfirmed { get; set; } = false;
    }
}