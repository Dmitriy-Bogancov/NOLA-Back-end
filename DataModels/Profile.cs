using Microsoft.AspNetCore.Identity;

namespace NOLA_API.DataModels
{
    public class Profile : IdentityUser
    {
        public string Image { get; set; } = "";
    }
}