using Dumbogram.Api.Persistence.Context.Application.Configurations.Chats;
using Dumbogram.Api.Persistence.Context.Application.Entities.Users;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Api.Persistence.Context.Application.Entities.Chats;

[EntityTypeConfiguration(typeof(ChatMembershipConfiguration))]
public class ChatMembership : BaseEntity
{
    public Guid ChatId { get; }
    public Chat Chat { get; set; } = null!;

    public Guid MemberId { get; }
    public UserProfile MemberProfile { get; set; } = null!;

    public MembershipStatus MembershipStatus { get; set; }
}