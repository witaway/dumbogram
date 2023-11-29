using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Messages.UserMessages;

[EntityTypeConfiguration(typeof(RegularUserMessageConfiguration))]
public class RegularUserMessage : UserMessage
{
    public string Content { get; set; } = null!;
    public int? RepliedMessageId { get; private set; }

    public Message? RepliedMessage { get; set; }
}

public class RegularUserMessageConfiguration : IEntityTypeConfiguration<RegularUserMessage>
{
    public void Configure(EntityTypeBuilder<RegularUserMessage> builder)
    {
        builder
            .HasOne(message => message.RepliedMessage)
            .WithMany(repliedMessage => repliedMessage.Replies)
            .HasForeignKey(message => new { message.ChatId, message.RepliedMessageId })
            .HasPrincipalKey(repliedMessage => new { repliedMessage.ChatId, repliedMessage.Id });

        builder.Property(message => message.Content).HasMaxLength(2048);
    }
}