using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Api.Persistence.Context.Application.Configurations.Messages;

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