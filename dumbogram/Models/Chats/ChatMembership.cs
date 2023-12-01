using Dumbogram.Models.Base;
using Dumbogram.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Chats;

public enum MembershipStatus
{
    Joined = 0,
    Leaved = 1,
    Banned = 2
}

[EntityTypeConfiguration(typeof(ChatMembershipConfiguration))]
public class ChatMembership : BaseEntity
{
    public Guid ChatId { get; private set; }
    public Chat Chat { get; set; } = null!;

    public Guid MemberId { get; private set; }
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