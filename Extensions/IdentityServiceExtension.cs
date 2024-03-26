using System.Text;
using NOLA_API.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using NOLA_API.Infrastructure.Security;
using NOLA_API.Services;

namespace NOLA_API.Extensions
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            opt.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<DataContext>();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("IsActivityHost", policy =>
            {
                policy.Requirements.Add(new IsHostRequirement());
            });
        });
        services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();
        services.AddScoped<TokenService>();
        
        return services;
    }
    }
}