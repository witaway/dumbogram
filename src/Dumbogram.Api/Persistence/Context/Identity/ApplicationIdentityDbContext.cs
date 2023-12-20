using Dumbogram.Api.Persistence.Context.Identity.Configurations;
using Dumbogram.Api.Persistence.Context.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Api.Persistence.Context.Identity;

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