using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Api.Models.Files.FileTypes;

[EntityTypeConfiguration(typeof(FileVideoConfiguration))]
public class FileVideo : File
{
    public FileVideo(File file, FileVideoMetadata metadata)
        : this(file)
    {
        Metadata = metadata;
    }

    public FileVideo(File file)
        : base(file)
    {
    }

    public FileVideo()
    {
    }

    public FileVideoMetadata Metadata { get; set; }
}

public class FileVideoConfiguration : IEntityTypeConfiguration<FileVideo>
{
    public void Configure(EntityTypeBuilder<FileVideo> builder)
    {
        builder.OwnsOne(
            file => file.Metadata,
            ownedNavigationBuilder => { ownedNavigationBuilder.ToJson("video_metadata"); }
        );
    }
}