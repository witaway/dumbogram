using Dumbogram.Api.Persistence.Context.Application.Entities.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Api.Persistence.Context.Application.Configurations.Chats;

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