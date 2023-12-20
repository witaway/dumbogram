using Dumbogram.Api.Persistence.Context.Application.Configurations.Files;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Api.Persistence.Context.Application.Entities.Files;

[EntityTypeConfiguration(typeof(FileConfiguration))]
public class FileRecord : BaseEntity
{
    public FileRecord()
    {
    }

    protected FileRecord(FileRecord other)
    {
        Id = other.Id;
        FilesGroupId = other.FilesGroupId;
        OriginalFileName = other.OriginalFileName;
        StoredFileName = other.StoredFileName;
        MimeType = other.MimeType;
        FileSize = other.FileSize;
    }

    public Guid Id { get; }
    public Guid? FilesGroupId { get; }
    public string? OriginalFileName { get; set; }
    public string StoredFileName { get; set; } = null!;
    public string MimeType { get; set; } = null!;
    public long FileSize { get; set; }

    public FileType Type { get; set; }

    public FileMetadata? Metadata { get; set; }

    public FilesGroup? FilesGroup { get; set; }
}