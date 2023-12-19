using Dumbogram.Api.Database.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Api.Database;

public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationIdentityUser>
{
    public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ApplicationIdentityRoleConfiguration());
        base.OnModelCreating(builder);
    }
}