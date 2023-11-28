using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Messages.UserMessages;

[EntityTypeConfiguration(typeof(UserMessageConfiguration))]
public class UserMessage : Message
{
    public IEnumerable<ForwardUserMessage> Forwards { get; set; } = null!;
}

public class UserMessageConfiguration : IEntityTypeConfiguration<UserMessage>
{
    public void Configure(EntityTypeBuilder<UserMessage> builder)
    {
        builder
            .HasMany(message => message.Forwards)
            .WithOne(forwardMessage => forwardMessage.ForwardedMessage)
            .HasForeignKey(forwardMessage => new { forwardMessage.ForwardedChatId, forwardMessage.ForwardedMessageId })
            .HasPrincipalKey(message => new { message.ChatId, message.Id });
    }
}