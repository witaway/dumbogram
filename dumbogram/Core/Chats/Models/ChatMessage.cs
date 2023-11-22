using Dumbogram.Common.Models;
using Dumbogram.Core.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Core.Chats.Models;

public enum MessageType
{
    NORMAL = 0,
    JOINED = 1,
    LEFT = 2,
    CHANGED_TITLE = 3,
    CHANGED_DESCRIPTION = 4,
    CHANGED_AVATAR = 5
}

[EntityTypeConfiguration(typeof(ChatMessageConfiguration))]
public class ChatMessage : BaseEntity
{
    public int Id { get; private set; }

    public Guid ChatId { get; private set; }
    public Chat Chat { get; } = null!;

    public Guid SenderId { get; private set; }
    public UserProfile SenderProfile { get; } = null!;

    public MessageType MessageType { get; set; }
    public string? Message { get; set; }
}

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        // Keys
        builder.HasKey(message => new { message.ChatId, message.Id });

        // Indexes
        builder.HasIndex(message => message.ChatId);
        builder.HasIndex(message => message.SenderId);
        builder.HasIndex(message => message.CreatedDate);

        // Relations
        builder
            .HasOne(message => message.SenderProfile)
            .WithMany(profile => profile.Messages)
            .HasForeignKey(message => message.SenderId)
            .HasPrincipalKey(profile => profile.UserId);

        builder
            .HasOne(message => message.Chat)
            .WithMany(chat => chat.Messages)
            .HasForeignKey(message => message.ChatId)
            .HasPrincipalKey(chat => chat.Id);

        // Constraints
        builder.Property(message => message.Message).HasMaxLength(1024);
    }
}