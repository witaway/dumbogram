using Dumbogram.Api.Persistence.Context.Application.Entities.Chats;
using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;
using Dumbogram.Api.Persistence.Context.Application.Entities.Users;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;
using Dumbogram.Api.Persistence.Infrastructure.Extensions;
using Dumbogram.Api.Persistence.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Api.Persistence.Context.Application;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context) : base(context)
    {
    }

    // User-related
    public DbSet<UserProfile> UserProfiles { get; set; }

    // Chats-related
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatMemberPermission> ChatMemberPermissions { get; set; }
    public DbSet<ChatMembership> ChatMemberships { get; set; }

    // Messages-related
    public DbSet<Message> Messages { get; set; }
    public DbSet<UserMessage> UserMessages { get; set; }
    public DbSet<SystemMessage> SystemMessages { get; set; }

    // Files-related
    public DbSet<FilesGroup> FilesGroups { get; set; }
    public DbSet<FileRecord> FilesRecords { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(
            new SoftDeleteInterceptor(),
            new TrackUpdatesInterceptor()
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyGlobalFilters<ISoftDelete>(e => e.DeletedDate == null);

        modelBuilder.HasPostgresEnum<FileType>();
        modelBuilder.HasPostgresEnum<SystemMessageType>();
        modelBuilder.HasPostgresEnum<FilesGroupType>();
    }
}