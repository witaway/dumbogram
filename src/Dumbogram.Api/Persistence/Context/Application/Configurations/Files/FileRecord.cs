using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Api.Persistence.Context.Application.Configurations.Files;

public class FileConfiguration : IEntityTypeConfiguration<FileRecord>
{
    public void Configure(EntityTypeBuilder<FileRecord> builder)
    {
        builder.ToTable("file");

        // Key
        builder.HasKey(file => file.Id);

        // Owns
        builder.OwnsOne(
            file => file.Metadata,
            ownedNavigationBuilder => { ownedNavigationBuilder.ToJson("metadata"); }
        );

        // Relations
        builder
            .HasOne(file => file.FilesGroup)
            .WithMany(filesGroup => filesGroup.Files)
            .HasForeignKey(file => file.FilesGroupId)
            .HasPrincipalKey(filesGroup => filesGroup.Id);

        // Properties
        builder
            .Property(file => file.Id)
            .HasColumnName("id");

        builder
            .Property(file => file.Type)
            .HasColumnName("type")
            .HasDefaultValue(FileType.Unknown);

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