﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Messages;

[EntityTypeConfiguration(typeof(RegularUserMessageConfiguration))]
public class UserMessage : Message
{
    public UserMessageContent Content { get; set; } = null!;

    public int? RepliedMessageId { get; set; }

    public Message? RepliedMessage { get; set; }
}

public class RegularUserMessageConfiguration : IEntityTypeConfiguration<UserMessage>
{
    public void Configure(EntityTypeBuilder<UserMessage> builder)
    {
        // MessageContent is JSON-property
        builder.OwnsOne(
            userMessage => userMessage.Content,
            ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
                ownedNavigationBuilder.Property(content => content.Text).HasMaxLength(2048);
            }
        );

        builder
            .HasOne(message => message.RepliedMessage)
            .WithMany(repliedMessage => repliedMessage.Replies)
            .HasForeignKey(message => new { message.ChatId, message.RepliedMessageId })
            .HasPrincipalKey(repliedMessage => new { repliedMessage.ChatId, repliedMessage.Id });
    }
}