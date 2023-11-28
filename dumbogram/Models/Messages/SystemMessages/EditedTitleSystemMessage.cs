using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Messages.SystemMessages;

[EntityTypeConfiguration(typeof(EditedTitleSystemMessageConfiguration))]
public class EditedTitleSystemMessage : SystemMessage
{
    public string NewContent { get; set; } = null!;
}

public class EditedTitleSystemMessageConfiguration : IEntityTypeConfiguration<EditedTitleSystemMessage>
{
    public void Configure(EntityTypeBuilder<EditedTitleSystemMessage> builder)
    {
    }
}