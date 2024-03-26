using System.Security.Claims;
using NOLA_API.Interfaces;

namespace NOLA_API.Infrastructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserAccessor(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetUsername()
        {
            return _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;
        }
    }
}