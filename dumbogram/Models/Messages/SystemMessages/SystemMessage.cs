using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Messages.SystemMessages;

[EntityTypeConfiguration(typeof(SystemMessageConfiguration))]
public class SystemMessage : Message
{
}

public class SystemMessageConfiguration : IEntityTypeConfiguration<SystemMessage>
{
    public void Configure(EntityTypeBuilder<SystemMessage> builder)
    {
    }
}