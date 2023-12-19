using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Api.Models.Files.FileTypes;

[EntityTypeConfiguration(typeof(FilePhotoConfiguration))]
public class FilePhoto : File
{
    public FilePhoto(File file, FilePhotoMetadata metadata)
        : this(file)
    {
        Metadata = metadata;
    }

    public FilePhoto(File file)
        : base(file)
    {
    }

    public FilePhoto()
    {
    }

    public FilePhotoMetadata Metadata { get; set; }
}

public class FilePhotoConfiguration : IEntityTypeConfiguration<FilePhoto>
{
    public void Configure(EntityTypeBuilder<FilePhoto> builder)
    {
        builder.OwnsOne(
            file => file.Metadata,
            ownedNavigationBuilder => { ownedNavigationBuilder.ToJson("photo_metadata"); }
        );
    }
}