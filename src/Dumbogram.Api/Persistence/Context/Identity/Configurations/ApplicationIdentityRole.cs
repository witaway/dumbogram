using System.Reflection;
using Dumbogram.Api.Persistence.Context.Identity.Enumerations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Api.Persistence.Context.Identity.Configurations;

public class ApplicationIdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        SyncRoles(builder);
    }

    private static void SyncRoles(EntityTypeBuilder<IdentityRole> builder)
    {
        var allHardCodedRoles = typeof(UserRoles)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField)
            .Where(f => f.FieldType == typeof(string))
            .Select(f => (string)f.GetValue(null)!)
            .ToList();

        allHardCodedRoles.ForEach(roleName =>
        {
            var role = new IdentityRole
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            };
            builder.HasData(role);
        });
    }
}