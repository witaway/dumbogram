using Dumbogram.Api.Database.Extensions;
using Dumbogram.Api.Database.Interceptors;
using Dumbogram.Api.Models.Base;
using Dumbogram.Api.Models.Chats;
using Dumbogram.Api.Models.Files;
using Dumbogram.Api.Models.Files.FileTypes;
using Dumbogram.Api.Models.Messages;
using Dumbogram.Api.Models.Users;
using Microsoft.EntityFrameworkCore;
using File = Dumbogram.Api.Models.Files.File;

namespace Dumbogram.Api.Database;

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
    public DbSet<File> Files { get; set; }

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

        modelBuilder.HasPostgresEnum<SystemMessageType>();
        modelBuilder.HasPostgresEnum<FilesGroupType>();

        // We don't need public DbSets for all those system message types
        modelBuilder.Entity<FileDocument>();
        modelBuilder.Entity<FileAnimation>();
        modelBuilder.Entity<FilePhoto>();
        modelBuilder.Entity<FileVideo>();
    }
}