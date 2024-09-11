using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace NOLA_API.Infrastructure.Security;

public class IsHostRequirement : IAuthorizationRequirement
{
}

public class IsHostRequirementHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
    : AuthorizationHandler<IsHostRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        IsHostRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Task.CompletedTask;

        var activityId = Guid.Parse(httpContextAccessor.HttpContext?.Request.RouteValues
            .SingleOrDefault(x => x.Key == "id").Value?.ToString());
        var attendee = dbContext.AdsVistors.AsNoTracking()
            .SingleOrDefaultAsync(x => x.AppUserId == userId && x.AdvertisementId == activityId).Result;
        if (attendee == null) return Task.CompletedTask;
        if (attendee.IsOwner) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}