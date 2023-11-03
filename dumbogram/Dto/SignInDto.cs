using System.ComponentModel.DataAnnotations;

namespace Dumbogram.Dto;


public class SignInDto
{
    // Todo: Make required one field at once
    public String Username { get; set; }
    public String Email { get; set; }
 
    [StringLength(255)]
    [Required]
    public String Password { get; set; }
}