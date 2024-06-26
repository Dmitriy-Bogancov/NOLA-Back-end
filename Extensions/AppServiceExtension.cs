using MediatR;
using Microsoft.EntityFrameworkCore;
using NOLA_API.Application.Advertisements;
using NOLA_API.Infrastructure.Messages;
using NOLA_API.Infrastructure.Security;
using NOLA_API.Interfaces;
using NOLA_API.Services;

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
                var envDb = System.Environment.GetEnvironmentVariable("CONNECTION_STRING", EnvironmentVariableTarget.Machine);
                var configDb = config.GetConnectionString("Remote");
                opt.UseNpgsql(envDb ?? configDb);
                //opt.UseSqlite(configDb);
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

            services.AddScoped<IEmailService, EmailService>();
            services.Configure<EmailConfiguration>(config.GetSection("EmailConfiguration"));

            return services;
        }
    }
}