using Dumbogram.Core.User.Models;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context) : base(context)
    {
    }

    public DbSet<UserProfile> UserProfiles { get; set; }
}