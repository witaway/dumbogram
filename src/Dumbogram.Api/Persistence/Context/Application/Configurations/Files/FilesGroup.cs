using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dumbogram.Api.Persistence.Context.Application.Configurations.Files;

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