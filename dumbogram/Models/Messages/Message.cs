using Dumbogram.Infrasctructure.Models;
using Dumbogram.Models.Chats;
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

    public Chat Chat { get; set; } = null!;
    public UserProfile SubjectProfile { get; set; } = null!;
    public IEnumerable<UserMessage> Replies { get; } = null!;
}

public class ChatMessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        // Keys
        builder.HasKey(message => new { message.ChatId, message.Id });
        // Todo: Use ROW_NUMBER() as here: https://stackoverflow.com/questions/27946892/auto-increment-id-based-on-composite-primary-key
        // Maybe should use HasComputedColumnSql or HasDefaultColumnSql
        builder.Property(message => message.Id).ValueGeneratedOnAdd();

        // Inheritance
        builder.HasDiscriminator<string>("message_type")
            .HasValue<SystemMessage>("system_message")
            .HasValue<UserMessage>("user_message");

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