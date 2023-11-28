using Dumbogram.Infrasctructure.Models;
using Dumbogram.Models.Chats;
using Dumbogram.Models.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Users;

[EntityTypeConfiguration(typeof(RolesConfiguration))]
public class UserProfile : BaseEntity
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string? Description { get; set; }

    public IEnumerable<Chat> OwnedChats { get; } = null!;
    public IEnumerable<Message> Messages { get; } = null!;
    public IEnumerable<ChatMembership> Memberships { get; } = null!;
    public IEnumerable<ChatMemberPermission> Permissions { get; } = null!;
}

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
            .WithOne(m => m.SubjectProfile)
            .HasForeignKey(m => m.SubjectId)
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

        // Constraints

        builder.Property(p => p.Username).HasMaxLength(32);
        builder.Property(p => p.Description).HasMaxLength(256);
    }
}