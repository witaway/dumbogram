using Dumbogram.Database.Extensions;
using Dumbogram.Database.Interceptors;
using Dumbogram.Models.Base;
using Dumbogram.Models.Chats;
using Dumbogram.Models.Messages;
using Dumbogram.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Database;

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
    }
}