using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Api.Models.Files.FileTypes;

[EntityTypeConfiguration(typeof(FileAnimationConfiguration))]
public class FileAnimation : File
{
    public FileAnimation(File file, FileAnimationMetadata metadata)
        : this(file)
    {
        Metadata = metadata;
    }

    public FileAnimation(File file)
        : base(file)
    {
    }

    public FileAnimation()
    {
    }

    public FileAnimationMetadata Metadata { get; set; }
}

public class FileAnimationConfiguration : IEntityTypeConfiguration<FileAnimation>
{
    public void Configure(EntityTypeBuilder<FileAnimation> builder)
    {
        builder.OwnsOne(
            file => file.Metadata,
            ownedNavigationBuilder => { ownedNavigationBuilder.ToJson("animation_metadata"); }
        );
    }
}