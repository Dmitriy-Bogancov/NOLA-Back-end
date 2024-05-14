
using NOLA_API.DataModels;
using NOLA_API.Domain;

namespace NOLA_API.Extensions
{
    public static class AddVisitorsExtension
    {
        public static Profile ToProfile(this AdVisitor visitor)
        {
            return new Profile
            {
                Id = visitor.AppUser.Id,
                UserName = visitor.AppUser.UserName,
                Image = visitor.AppUser.Image,
            };
        }
    }
}