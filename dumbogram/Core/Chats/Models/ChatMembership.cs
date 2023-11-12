using Dumbogram.Common.Models;
using Dumbogram.Core.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Core.Chats.Models;

public enum MembershipStatus
{
    Alive = 0,
    Leaved = 1,
    Banned = 2
}

[EntityTypeConfiguration(typeof(ChatMembershipConfiguration))]
public class ChatMembership : BaseEntity
{
    public Guid ChatId { get; set; }
    public Chat Chat { get; } = null!;

    public Guid MemberId { get; set; }
    public UserProfile MemberProfile { get; set; } = null!;

    public MembershipStatus MembershipStatus { get; set; }
}

public class ChatMembershipConfiguration : IEntityTypeConfiguration<ChatMembership>
{
    public void Configure(EntityTypeBuilder<ChatMembership> builder)
    {
        // Keys
        builder.HasKey(membership =>
            new { membership.ChatId, membership.MemberId }
        );
    }
}