using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Api.Database.Identity;

public static class UserRoles
{
    public const string Admin = "Admin";
    public const string User = "User";
    public const string Moderator = "Moderator";
}

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
            var role = new IdentityRole()
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
            };
            builder.HasData(role);
        });
    }
}