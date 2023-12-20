using Dumbogram.Api.Persistence.Context.Application.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Api.Persistence.Context.Application.Configurations.Users;

public class RolesConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        // Keys

        builder.HasKey(p => p.UserId);
        builder.HasAlternateKey(p => p.Username);

        // Relations

        builder
            .HasMany(p => p.OwnedChats)
            .WithOne(c => c.OwnerProfile)
            .HasForeignKey(c => c.OwnerId)
            .HasPrincipalKey(p => p.UserId);

        builder
            .HasMany(p => p.Messages)
            .WithOne(m => m.SenderProfile)
            .HasForeignKey(m => m.SenderId)
            .HasPrincipalKey(p => p.UserId);

        builder
            .HasMany(p => p.Memberships)
            .WithOne(m => m.MemberProfile)
            .HasForeignKey(m => m.MemberId)
            .HasPrincipalKey(p => p.UserId);

        builder
            .HasMany(p => p.Permissions)
            .WithOne(m => m.MemberProfile)
            .HasForeignKey(m => m.MemberId)
            .HasPrincipalKey(p => p.UserId);

        builder
            .HasMany(p => p.FilesGroups)
            .WithOne(m => m.Owner)
            .HasForeignKey(m => m.OwnerId)
            .HasPrincipalKey(p => p.UserId);

        // Constraints

        builder.Property(p => p.Username).HasMaxLength(32);
        builder.Property(p => p.Description).HasMaxLength(256);
    }
}