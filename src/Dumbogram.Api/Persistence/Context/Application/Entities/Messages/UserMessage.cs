using Dumbogram.Api.Persistence.Context.Application.Configurations.Messages;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Api.Persistence.Context.Application.Entities.Messages;

[EntityTypeConfiguration(typeof(RegularUserMessageConfiguration))]
public class UserMessage : Message
{
    // Sender Id is derived from Message, but it's required for UserMessage
    public new Guid SenderId { get; }

    public UserMessageContent Content { get; set; } = null!;

    public int? RepliedMessageId { get; set; }

    public Message? RepliedMessage { get; set; }
}