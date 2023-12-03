using Dumbogram.Models.Base;
using Dumbogram.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Models.Files;

[EntityTypeConfiguration(typeof(FilesGroupConfiguration))]
public class FilesGroup : BaseEntity
{
    public Guid Id { get; private set; }
    public Guid OwnerId { get; private set; }
    public FilesGroupType GroupType { get; set; }

    public UserProfile Owner { get; set; } = null!;
    public IList<File> Files { get; set; } = null!;
}

public class FilesGroupConfiguration : IEntityTypeConfiguration<FilesGroup>
{
    public void Configure(EntityTypeBuilder<FilesGroup> builder)
    {
        builder.ToTable("files_groups");

        // Key
        builder.HasKey(filesGroup => filesGroup.Id);

        // Relationships
        builder
            .HasMany(filesGroup => filesGroup.Files)
            .WithOne(file => file.FilesGroup)
            .HasForeignKey(file => file.FilesGroupId)
            .HasPrincipalKey(filesGroup => filesGroup.Id);

        builder
            .HasOne(filesGroup => filesGroup.Owner)
            .WithMany(owner => owner.FilesGroups)
            .HasForeignKey(filesGroup => filesGroup.OwnerId)
            .HasPrincipalKey(owner => owner.UserId);

        // Properties
        builder
            .Property(filesGroup => filesGroup.Id)
            .HasColumnName("id");

        builder
            .Property(filesGroup => filesGroup.OwnerId)
            .HasColumnName("owner_id");

        builder
            .Property(filesGroup => filesGroup.GroupType)
            .HasColumnName("group_type");
    }
}