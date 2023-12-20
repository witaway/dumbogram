using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Api.Persistence.Context.Application.Configurations.Messages;

public class ChatMessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        // Keys
        builder.HasKey(message => new { message.ChatId, message.Id });
        // Todo: Use ROW_NUMBER() as here: https://stackoverflow.com/questions/27946892/auto-increment-id-based-on-composite-primary-key
        // Maybe should use HasComputedColumnSql or HasDefaultColumnSql
        // Or use triggers such as: https://stackoverflow.com/questions/38927629/column-value-autoincrement-depending-on-another-column-value-entity-framework-co
        builder.Property(message => message.Id).ValueGeneratedOnAdd();

        // Inheritance
        builder.HasDiscriminator<string>("message_type")
            .HasValue<SystemMessage>("system_message")
            .HasValue<UserMessage>("user_message");

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

        builder
            .HasMany(message => message.Replies)
            .WithOne(replyMessage => replyMessage.RepliedMessage)
            .HasForeignKey(replyMessage => new { replyMessage.ChatId, replyMessage.RepliedMessageId })
            .HasPrincipalKey(message => new { message.ChatId, message.Id });
    }
}