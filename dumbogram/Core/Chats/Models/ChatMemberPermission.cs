﻿using Dumbogram.Common.Models;
using Dumbogram.Core.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Core.Chats.Models;

public enum MembershipRight
{
    Owner = 0,
    Write = 1,
    ChangeTitle = 2,
    ChangeDescription = 3,
    ChangeAvatar = 4,
    BanHummer = 5,
    Immortal = 6,
    Invite = 7,
    Demote = 8
}

[EntityTypeConfiguration(typeof(ChatMemberPermissionConfiguration))]
public class ChatMemberPermission : BaseEntity
{
    public Guid ChatId { get; set; }
    public Chat Chat { get; } = null!;

    public Guid MemberId { get; set; }
    public UserProfile MemberProfile { get; set; } = null!;

    public MembershipRight MembershipRight { get; set; }
}

public class ChatMemberPermissionConfiguration : IEntityTypeConfiguration<ChatMemberPermission>
{
    public void Configure(EntityTypeBuilder<ChatMemberPermission> builder)
    {
        // Keys
        builder.HasKey(membership =>
            new { membership.ChatId, membership.MemberId }
        );
    }
}