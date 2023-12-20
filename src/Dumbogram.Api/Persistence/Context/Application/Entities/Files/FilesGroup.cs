using Dumbogram.Api.Persistence.Context.Application.Configurations.Files;
using Dumbogram.Api.Persistence.Context.Application.Entities.Users;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Api.Persistence.Context.Application.Entities.Files;

[EntityTypeConfiguration(typeof(FilesGroupConfiguration))]
public class FilesGroup : BaseEntity
{
    public Guid Id { get; }
    public Guid OwnerId { get; }
    public FilesGroupType GroupType { get; set; }

    public UserProfile Owner { get; set; } = null!;
    public IList<FileRecord> Files { get; set; } = null!;
}