using Dumbogram.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Files;

[EntityTypeConfiguration(typeof(FilesGroupConfiguration))]
public class FilesGroup : BaseEntity
{
    public Guid Id { get; set; }
    public IList<File> Files { get; set; }
}

public class FilesGroupConfiguration : IEntityTypeConfiguration<FilesGroup>
{
    public void Configure(EntityTypeBuilder<FilesGroup> builder)
    {
        // Key
        builder.HasKey(filesGroup => filesGroup.Id);

        // Relationships
        builder
            .HasMany(filesGroup => filesGroup.Files)
            .WithOne(file => file.FilesGroup)
            .HasForeignKey(file => file.FilesGroupId)
            .HasPrincipalKey(filesGroup => filesGroup.Id);
    }
}