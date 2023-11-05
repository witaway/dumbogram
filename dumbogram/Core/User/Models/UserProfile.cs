using System.ComponentModel.DataAnnotations;
using Dumbogram.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Core.User.Models;

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