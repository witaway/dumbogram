using Dumbogram.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Files;

[EntityTypeConfiguration(typeof(FileConfiguration))]
public class File : BaseEntity
{
    public Guid Id { get; private set; }
    public Guid? FilesGroupId { get; private set; }
    public Guid? ThumbnailId { get; }

    public string? OriginalFileName { get; set; }
    public string StoredFileName { get; set; } = null!;

    public string MimeType { get; set; } = null!;
    public int FileSize { get; set; }

    public FilesGroup? FilesGroup { get; set; }
    public FilePhoto? Thumbnail { get; set; }
}

public class FileConfiguration : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.ToTable("file");

        // Key
        builder.HasKey(file => file.Id);

        // Inheritance
        builder
            .HasDiscriminator<string>("file_type")
            .HasValue<FilePhoto>("photo")
            .HasValue<FileDocument>("document")
            .HasValue<FileVideo>("video")
            .HasValue<FileAnimation>("animation");

        // Relations
        builder
            .HasOne(file => file.FilesGroup)
            .WithMany(filesGroup => filesGroup.Files)
            .HasForeignKey(file => file.FilesGroupId)
            .HasPrincipalKey(filesGroup => filesGroup.Id);

        builder
            .OwnsOne(file => file.Thumbnail)
            .WithOwner()
            .HasForeignKey(e => e.Id)
            .HasPrincipalKey(e => e.ThumbnailId);

        // Properties
        builder
            .Property(file => file.Id)
            .HasColumnName("id");

        builder
            .Property(file => file.ThumbnailId)
            .HasColumnName("thumbnail_id");

        builder
            .Property(file => file.FilesGroupId)
            .HasColumnName("files_group_id");

        builder
            .Property(file => file.OriginalFileName)
            .HasColumnName("original_filename");

        builder
            .Property(file => file.StoredFileName)
            .HasColumnName("stored_filename");

        builder
            .Property(file => file.MimeType)
            .HasColumnName("mime_type");

        builder
            .Property(file => file.FileSize)
            .HasColumnName("file_size");
    }
}