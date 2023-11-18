using Dumbogram.Common.Models;
using Dumbogram.Core.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Core.Chats.Models;

public enum ChatVisibility
{
    Public = 0,
    Private = 1
}

[EntityTypeConfiguration(typeof(ChatConfiguration))]
public class Chat : BaseEntity
{
    public Guid Id { get; private set; }
    public Guid OwnerId { get; private set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public ChatVisibility ChatVisibility { get; set; }

    public UserProfile OwnerProfile { get; set; } = null!;
    public IEnumerable<ChatMessage> Messages { get; set; } = null!;
    public IEnumerable<ChatMembership> Memberships { get; set; } = null!;
    public IEnumerable<ChatMemberPermission> Permissions { get; set; } = null!;
}

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        // Keys
        builder.HasKey(chat => chat.Id);

        // Relations
        builder
            .HasOne(chat => chat.OwnerProfile)
            .WithMany(profile => profile.OwnedChats)
            .HasForeignKey(chat => chat.OwnerId)
            .HasPrincipalKey(profile => profile.UserId);

        builder
            .HasMany(chat => chat.Memberships)
            .WithOne(membership => membership.Chat)
            .HasForeignKey(membership => membership.ChatId)
            .HasPrincipalKey(chat => chat.Id);

        builder
            .HasMany(chat => chat.Permissions)
            .WithOne(permission => permission.Chat)
            .HasForeignKey(membership => membership.ChatId)
            .HasPrincipalKey(chat => chat.Id);

        // Constraints
        builder.Property(chat => chat.Title).HasMaxLength(64);
        builder.Property(chat => chat.Description).HasMaxLength(256);
    }
}