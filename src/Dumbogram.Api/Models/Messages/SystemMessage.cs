using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Api.Models.Messages;

[EntityTypeConfiguration(typeof(SystemMessageConfiguration))]
public class SystemMessage : Message
{
    public SystemMessageType SystemMessageType { get; set; }
    public SystemMessageDetails? SystemMessageDetails { get; set; }
}

public class SystemMessageConfiguration : IEntityTypeConfiguration<SystemMessage>
{
    public void Configure(EntityTypeBuilder<SystemMessage> builder)
    {
        // SystemMessageMetadata is JSON-property
        builder.OwnsOne(
            systemMessage => systemMessage.SystemMessageDetails,
            ownedNavigationBuilder => { ownedNavigationBuilder.ToJson(); }
        );
    }
}