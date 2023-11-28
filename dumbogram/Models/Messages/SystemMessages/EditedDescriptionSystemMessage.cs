using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Messages.SystemMessages;

[EntityTypeConfiguration(typeof(EditedDescriptionSystemMessageConfiguration))]
public class EditedDescriptionSystemMessage : SystemMessage
{
    public string NewContent { get; set; } = null!;
}

public class EditedDescriptionSystemMessageConfiguration : IEntityTypeConfiguration<EditedDescriptionSystemMessage>
{
    public void Configure(EntityTypeBuilder<EditedDescriptionSystemMessage> builder)
    {
    }
}