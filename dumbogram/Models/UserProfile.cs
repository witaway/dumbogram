using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Models;

[Index(nameof(Username), IsUnique = true)]
public class UserProfile : BaseEntity
{
    [Key]
    public Guid UserId { get; set; }

    [StringLength(32)]
    public string Username { get; set; }

    [StringLength(256)]
    public string Description { get; set; }

    public Guid AvatarMediaId { get; set; }
}