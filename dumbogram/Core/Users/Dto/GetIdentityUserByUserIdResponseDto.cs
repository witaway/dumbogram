using Dumbogram.Database.Identity;

namespace Dumbogram.Core.Users.Dto;

public class GetIdentityUserByUserIdResponseDto
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }

    public static GetIdentityUserByUserIdResponseDto MapFromModel(ApplicationIdentityUser user)
    {
        return new GetIdentityUserByUserIdResponseDto
        {
            Id = user.Id,
            Email = user.Email!,
            UserName = user.UserName!
        };
    }
}