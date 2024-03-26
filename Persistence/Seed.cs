using Microsoft.AspNetCore.Identity;
using NOLA_API.DataModels;
using NOLA_API.Domain;

namespace NOLA_API.Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context,
            UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any() && !context.Ads.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        UserName = "bob",
                        Email = "bob@test.com"
                    },
                    new AppUser
                    {
                        UserName = "jane",
                        Email = "jane@test.com"
                    },
                    new AppUser
                    {
                        UserName = "tom",
                        Email = "tom@test.com"
                    },
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }

                var ads = new List<Advertisement>
                {
                    new Advertisement
                    {
                        Title = "Advertisement 1",
                        CreatedAt = DateTime.UtcNow.AddMonths(-2),
                        Description = "Advertise 2 months ago",
                        Visitors = new List<AdVisitor>
                        {
                            new AdVisitor
                            {
                                AppUser = users[0],
                                IsOwner = true
                            }
                        }
                    },
                    new Advertisement
                    {
                        Title = "Advertisement 1",
                        CreatedAt = DateTime.UtcNow.AddMonths(-1),
                        Description = "Advertise 1 month ago",
                        Visitors = new List<AdVisitor>
                        {
                            new AdVisitor
                            {
                                AppUser = users[0],
                                IsOwner = true
                            },
                            new AdVisitor
                            {
                                AppUser = users[1],
                                IsOwner = false
                            },
                        }
                    },
                    new Advertisement
                    {
                        Title = "Adv 3",
                        CreatedAt = DateTime.UtcNow.AddDays(-10),
                        Description = "ADv 10 days ago",
                        Visitors = new List<AdVisitor>
                        {
                            new AdVisitor
                            {
                                AppUser = users[2],
                                IsOwner = true
                            },
                            new AdVisitor
                            {
                                AppUser = users[1],
                                IsOwner = false
                            },
                        }
                    },
                    new Advertisement
                    {
                        Title = "adv 5 day ago",
                        CreatedAt = DateTime.UtcNow.AddMonths(2),
                        Description = "Activity 2 months in future",
                        Visitors = new List<AdVisitor>
                        {
                            new AdVisitor
                            {
                                AppUser = users[0],
                                IsOwner = true
                            },
                            new AdVisitor
                            {
                                AppUser = users[2],
                                IsOwner = false
                            },
                        }
                    },
                    new Advertisement
                    {
                        Title = "ad 3",
                        CreatedAt = DateTime.UtcNow.AddDays(-5),
                        Description = "adv 5 days ago",
                        Visitors = new List<AdVisitor>
                        {
                            new AdVisitor
                            {
                                AppUser = users[1],
                                IsOwner = true                            
                            },
                            new AdVisitor
                            {
                                AppUser = users[0],
                                IsOwner = false                            
                            },
                        }
                    },
                    new Advertisement
                    {
                        Title = "Future ad 4",
                        CreatedAt = DateTime.UtcNow.AddMonths(1),
                        Description = "adv 1 months in future",
                        Visitors =new List<AdVisitor>
                        {
                            new AdVisitor
                            {
                                AppUser = users[1],
                                IsOwner = true                            
                            }
                        }
                    },
                    new Advertisement
                    {
                        Title = "Future ad 5",
                        CreatedAt = DateTime.UtcNow.AddMonths(5),
                        Description = "ad 5 months in future",
                        Visitors = new List<AdVisitor>
                        {
                            new AdVisitor
                            {
                                AppUser = users[0],
                                IsOwner = true                            
                            },
                            new AdVisitor
                            {
                                AppUser = users[1],
                                IsOwner = false                            
                            },
                        }
                    },
                    new Advertisement
                    {
                        Title = "Future ad 6",
                        CreatedAt = DateTime.UtcNow.AddMonths(6),
                        Description = "ad 6 months in future",
                        Visitors =new List<AdVisitor>
                        {
                            new AdVisitor
                            {
                                AppUser = users[2],
                                IsOwner = true                            
                            },
                            new AdVisitor
                            {
                                AppUser = users[1],
                                IsOwner = false                            
                            },
                        }
                    },
                    new Advertisement
                    {
                        Title = "Future ad 7",
                        CreatedAt = DateTime.UtcNow.AddDays(7),
                        Description = "ad 7 d in future",
                        Visitors =new List<AdVisitor>
                        {
                            new AdVisitor
                            {
                                AppUser = users[0],
                                IsOwner = true                            
                            },
                            new AdVisitor
                            {
                                AppUser = users[2],
                                IsOwner = false                            
                            },
                        }
                    },
                    new Advertisement
                    {
                        Title = "Future ad 8",
                        CreatedAt = DateTime.UtcNow.AddDays(18),
                        Description = "ad 18 d in future",
                        Visitors =new List<AdVisitor>
                        {
                            new AdVisitor
                            {
                                AppUser = users[2],
                                IsOwner = true                            
                            },
                            new AdVisitor
                            {
                                AppUser = users[1],
                                IsOwner = false                            
                            },
                        }
                    }
                };

                await context.Ads.AddRangeAsync(ads);
                await context.SaveChangesAsync();
            }
        }
    }
}