using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Messages.SystemMessages;

[EntityTypeConfiguration(typeof(LeftSystemMessageConfiguration))]
public class LeftSystemMessage : SystemMessage
{
}

public class LeftSystemMessageConfiguration : IEntityTypeConfiguration<LeftSystemMessage>
{
    public void Configure(EntityTypeBuilder<LeftSystemMessage> builder)
    {
    }
}