using Dumbogram.Api.Persistence.Context.Application.Configurations.Users;
using Dumbogram.Api.Persistence.Context.Application.Entities.Chats;
using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;
using Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Api.Persistence.Context.Application.Entities.Users;

[EntityTypeConfiguration(typeof(RolesConfiguration))]
public class UserProfile : BaseEntity
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string? Description { get; set; }

    public IEnumerable<Chat> OwnedChats { get; } = null!;
    public IEnumerable<Message> Messages { get; } = null!;
    public ICollection<FilesGroup> FilesGroups { get; } = null!;
    public IEnumerable<ChatMembership> Memberships { get; } = null!;
    public IEnumerable<ChatMemberPermission> Permissions { get; } = null!;
}