using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Messages.SystemMessages;

[EntityTypeConfiguration(typeof(JoinedSystemMessageConfiguration))]
public class JoinedSystemMessage : SystemMessage
{
}

public class JoinedSystemMessageConfiguration : IEntityTypeConfiguration<JoinedSystemMessage>
{
    public void Configure(EntityTypeBuilder<JoinedSystemMessage> builder)
    {
    }
}