using System.Reflection;
using Dumbogram.Core.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Database.Configuration.Identity;

public class RolesConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        SyncRoles(builder);
    }

    private void SyncRoles(EntityTypeBuilder<IdentityRole> builder)
    {
        // Convince that all roles declared in Enum Class are exists in Database
        // For example, If we have:
        //
        //      public static class UserRoles
        //      {
        //          public const string Admin = "Admin";
        //          public const string User = "User";
        //          public const string Moderator = "Moderator";
        //      }
        // 
        // We will DEFINITELY have in Database:
        //      IdentityRole("Admin"), IdentityRole("User"), IdentityRole("Moderator")

        var allHardCodedRoles = typeof(UserRoles)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField)
            .Where(f => f.FieldType == typeof(string))
            .Select(f => (string)f.GetValue(null)!)
            .ToList();

        foreach (var roleName in allHardCodedRoles)
        {
            var role = new IdentityRole(roleName);
            builder.HasData(role);
        }
    }
}