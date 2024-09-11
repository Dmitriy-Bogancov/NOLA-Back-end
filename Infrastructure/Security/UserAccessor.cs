using System.Security.Claims;
using NOLA_API.Interfaces;

namespace NOLA_API.Infrastructure.Security;

public class UserAccessor(IHttpContextAccessor contextAccessor) : IUserAccessor
{
    public string GetUsername()
    {
        return contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;
    }
}