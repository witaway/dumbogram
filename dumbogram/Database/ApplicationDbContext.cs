using Dumbogram.Core.Chats.Models;
using Dumbogram.Core.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context) : base(context)
    {
    }

    // User models
    public DbSet<UserProfile> UserProfiles { get; set; }

    // Chat models
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatMemberPermission> ChatMemberPermissions { get; set; }
    public DbSet<ChatMembership> ChatMemberships { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
}