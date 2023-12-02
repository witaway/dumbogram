using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Files;

[EntityTypeConfiguration(typeof(FilePhotoConfiguration))]
public class FilePhoto : File
{
    public int Width { get; set; }
    public int Height { get; set; }
}

public class FilePhotoConfiguration : IEntityTypeConfiguration<FilePhoto>
{
    public void Configure(EntityTypeBuilder<FilePhoto> builder)
    {
        builder
            .Property(e => e.Width)
            .HasColumnName("width");

        builder
            .Property(e => e.Height)
            .HasColumnName("height");
    }
}