using Dumbogram.Models.Chats;
using Dumbogram.Models.Messages;
using Dumbogram.Models.Messages.SystemMessages;
using Dumbogram.Models.Messages.UserMessages;
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
    public DbSet<RegularUserMessage> RegularUserMessages { get; set; }
    public DbSet<ForwardUserMessage> ForwardUserMessages { get; set; }
    public DbSet<SystemMessage> SystemMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<SystemMessageType>();
    }
}