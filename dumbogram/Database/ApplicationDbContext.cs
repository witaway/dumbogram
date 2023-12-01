using Dumbogram.Infrasctructure.Models;
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

    // Todo: FIX THE NOT WORKING SHIT. UpdatedDate and CreatedDate are always -Infinity.
    // But it's better for now, than conflicting with not-null constraint for these fields
    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e =>
                e.Entity is BaseEntity && (
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified
                )
            );

        foreach (var entityEntry in entries)
        {
            var entity = (BaseEntity)entityEntry.Entity;

            entity.UpdatedDate = DateTime.Now;

            if (entityEntry.State == EntityState.Added)
            {
                entity.CreatedDate = DateTime.Now;
            }
        }

        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<SystemMessageType>();
    }
}