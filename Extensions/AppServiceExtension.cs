

using MediatR;
using Microsoft.EntityFrameworkCore;
using NOLA_API.Application.Advertisements;
using NOLA_API.Infrastructure.Security;
using NOLA_API.Interfaces;

namespace NOLA_API.Extensions
{
    public static class AppServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<DataContext>(opt =>
            {
                var envDb = System.Environment.GetEnvironmentVariable("CONNECTION_STRING");
                var configDb = config.GetConnectionString("DefaultConnection");
                opt.UseSqlite();
            });
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            services.AddMediatR(typeof(Show.Handler));
            services.AddHttpContextAccessor();
            services.AddScoped<IUserAccessor, UserAccessor>();

            return services;
        }
    }
}