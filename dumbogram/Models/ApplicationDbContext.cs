using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context) : base(context)
    {
    }

    public DbSet<UserProfile> UserProfiles { get; set; }
}