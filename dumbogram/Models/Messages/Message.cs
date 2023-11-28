using Dumbogram.Infrasctructure.Models;
using Dumbogram.Models.Chats;
using Dumbogram.Models.Messages.UserMessages;
using Dumbogram.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Messages;

[EntityTypeConfiguration(typeof(ChatMessageConfiguration))]
public class Message : BaseEntity
{
    public int Id { get; private set; }
    public Guid ChatId { get; private set; }
    public Guid SubjectId { get; private set; }

    public Chat Chat { get; } = null!;
    public UserProfile SubjectProfile { get; } = null!;
    public IEnumerable<RegularUserMessage> Replies { get; set; } = null!;
}

public class ChatMessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        // Keys
        builder.HasKey(message => new { message.ChatId, message.Id });

        // Indexes
        builder.HasIndex(message => message.ChatId);
        builder.HasIndex(message => message.SubjectId);
        builder.HasIndex(message => message.CreatedDate);

        // Relations
        builder
            .HasOne(message => message.SubjectProfile)
            .WithMany(profile => profile.Messages)
            .HasForeignKey(message => message.SubjectId)
            .HasPrincipalKey(profile => profile.UserId);

        builder
            .HasOne(message => message.Chat)
            .WithMany(chat => chat.Messages)
            .HasForeignKey(message => message.ChatId)
            .HasPrincipalKey(chat => chat.Id);

        builder
            .HasMany(message => message.Replies)
            .WithOne(replyMessage => replyMessage.RepliedMessage)
            .HasForeignKey(replyMessage => new { replyMessage.ChatId, replyMessage.RepliedMessageId })
            .HasPrincipalKey(message => new { message.ChatId, message.Id });
    }
}