using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Messages.UserMessages;

[EntityTypeConfiguration(typeof(ForwardUserMessageConfiguration))]
public class ForwardUserMessage : UserMessage
{
    public Guid ForwardedChatId { get; private set; }
    public int ForwardedMessageId { get; private set; }

    public UserMessage ForwardedMessage { get; set; } = null!;
}

public class ForwardUserMessageConfiguration : IEntityTypeConfiguration<ForwardUserMessage>
{
    public void Configure(EntityTypeBuilder<ForwardUserMessage> builder)
    {
        builder
            .HasOne(forwardMessage => forwardMessage.ForwardedMessage)
            .WithMany(forwardedMessage => forwardedMessage.Forwards)
            .HasForeignKey(forwardMessage => new { forwardMessage.ForwardedChatId, forwardMessage.ForwardedMessageId })
            .HasPrincipalKey(forwardedMessage => new { forwardedMessage.ChatId, forwardedMessage.Id });
    }
}