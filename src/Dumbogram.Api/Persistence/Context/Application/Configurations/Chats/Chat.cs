using Dumbogram.Api.Persistence.Context.Application.Entities.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Api.Persistence.Context.Application.Configurations.Chats;

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