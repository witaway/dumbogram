using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Files;

[EntityTypeConfiguration(typeof(FileVideoConfiguration))]
public class FileVideo : File
{
    public FileVideo(File file)
        : base(file)
    {
    }

    public FileVideo()
    {
    }

    public int Width { get; set; }
    public int Height { get; set; }
    public int Duration { get; set; }
}

public class FileVideoConfiguration : IEntityTypeConfiguration<FileVideo>
{
    public void Configure(EntityTypeBuilder<FileVideo> builder)
    {
        builder
            .Property(e => e.Width)
            .HasColumnName("width");

        builder
            .Property(e => e.Height)
            .HasColumnName("height");

        builder
            .Property(e => e.Duration)
            .HasColumnName("duration");
    }
}