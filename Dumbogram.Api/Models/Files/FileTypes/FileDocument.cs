using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Api.Models.Files.FileTypes;

[EntityTypeConfiguration(typeof(FileDocumentConfiguration))]
public class FileDocument : File
{
    public FileDocument(File file, FileDocumentMetadata metadata)
        : this(file)
    {
        Metadata = metadata;
    }

    public FileDocument(File file)
        : base(file)
    {
    }

    public FileDocument()
    {
    }

    public FileDocumentMetadata Metadata { get; set; }
}

public class FileDocumentConfiguration : IEntityTypeConfiguration<FileDocument>
{
    public void Configure(EntityTypeBuilder<FileDocument> builder)
    {
        builder.OwnsOne(
            file => file.Metadata,
            ownedNavigationBuilder => { ownedNavigationBuilder.ToJson("document_metadata"); }
        );
    }
}