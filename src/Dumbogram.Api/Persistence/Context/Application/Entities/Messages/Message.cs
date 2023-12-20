using Dumbogram.Api.Persistence.Context.Application.Configurations.Messages;
using Dumbogram.Api.Persistence.Context.Application.Entities.Chats;
using Dumbogram.Api.Persistence.Context.Application.Entities.Users;
using Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Api.Persistence.Context.Application.Entities.Messages;

[EntityTypeConfiguration(typeof(ChatMessageConfiguration))]
public class Message : BaseEntity
{
    public int Id { get; set; }
    public Guid ChatId { get; set; }
    public Guid? SenderId { get; set; }

    public Chat Chat { get; set; } = null!;
    public UserProfile? SenderProfile { get; set; }
    public IEnumerable<UserMessage> Replies { get; } = null!;
}