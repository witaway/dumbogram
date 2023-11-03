using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Models;

public enum UserRole
{
    User = 0,
    Admin = 1,
    Moderator = 2
}

public enum UserStatus
{
    Alive = 0,
    Banned = 1,
    Deleted = 2
}

[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class User : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(255)]
    public string Username { get; set; }

    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; }

    [StringLength(255)]
    public string Password { get; set; }

    [DefaultValue(UserRole.User)]
    public UserRole Role { get; set; }

    [DefaultValue(UserStatus.Alive)]
    public UserStatus Status { get; set; }
}