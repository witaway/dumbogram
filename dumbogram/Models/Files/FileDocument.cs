using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Files;

[EntityTypeConfiguration(typeof(FileDocumentConfiguration))]
public class FileDocument : File
{
    public FileDocument(File file)
        : base(file)
    {
    }

    public FileDocument()
    {
    }
}

public class FileDocumentConfiguration : IEntityTypeConfiguration<FileDocument>
{
    public void Configure(EntityTypeBuilder<FileDocument> builder)
    {
    }
}