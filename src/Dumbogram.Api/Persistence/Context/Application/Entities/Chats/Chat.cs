using Dumbogram.Api.Persistence.Context.Application.Configurations.Chats;
using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;
using Dumbogram.Api.Persistence.Context.Application.Entities.Users;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Api.Persistence.Context.Application.Entities.Chats;

[EntityTypeConfiguration(typeof(ChatConfiguration))]
public class Chat : BaseEntity
{
    public Guid Id { get; }
    public Guid OwnerId { get; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public ChatVisibility ChatVisibility { get; set; }

    public UserProfile OwnerProfile { get; set; } = null!;
    public IEnumerable<Message> Messages { get; set; } = null!;
    public IEnumerable<ChatMembership> Memberships { get; set; } = null!;
    public IEnumerable<ChatMemberPermission> Permissions { get; set; } = null!;
}