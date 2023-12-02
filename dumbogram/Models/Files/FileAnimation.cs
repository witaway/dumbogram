using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Files;

[EntityTypeConfiguration(typeof(FileAnimationConfiguration))]
public class FileAnimation : File
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int Duration { get; set; }
}

public class FileAnimationConfiguration : IEntityTypeConfiguration<FileAnimation>
{
    public void Configure(EntityTypeBuilder<FileAnimation> builder)
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