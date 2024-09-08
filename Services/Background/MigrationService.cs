using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NOLA_API.Domain;
using NOLA_API.Persistence;

namespace NOLA_API.Services.Background;

public class MigrationService(DataContext context, UserManager<AppUser> userManager, ILogger<MigrationService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await context.Database.MigrateAsync(cancellationToken: stoppingToken);
            await Seed.SeedData(context, userManager);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Migration error");
        }
    }
}