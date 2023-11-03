namespace Dumbogram.Dto;

public class UpdateProfileDto
{
    public String Name { get; set; }
    public String Description { get; set; }
    public Guid AvatarMediaId { get; set; }
}