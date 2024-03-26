
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NOLA_API.DataModels;
using NOLA_API.Domain;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Advertisement> Ads { get; set; }
    public DbSet<AdVisitor> AdsVistors { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AdVisitor>(x => 
            x.HasKey(aa => new { aa.AppUserId, aa.AdvertisementId }));
        builder.Entity<AdVisitor>()
            .HasOne(u => u.AppUser)
            .WithMany(a => a.Posts)
            .HasForeignKey(aa => aa.AppUserId);
        
        builder.Entity<AdVisitor>()
            .HasOne(u => u.Post)
            .WithMany(a => a.Visitors)
            .HasForeignKey(aa => aa.AdvertisementId);
    }
}