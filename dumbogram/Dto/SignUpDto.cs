using System.ComponentModel.DataAnnotations;

namespace Dumbogram.Dto;

public class SignUpDto
{
    [StringLength(255)]
    [Required]
    public String Username { get; set; }
    
    [StringLength(255)]
    [Required]
    public String Email { get; set; }
    
    [StringLength(255)]
    [Required]
    public String Password { get; set; }
    
    public UpdateProfileDto? Profile { get; set; }
}