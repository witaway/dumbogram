using Dumbogram.Database.Identity;

namespace Dumbogram.Application.Users.Controllers.Dto;

public class GetIdentityUserByUserIdResponse
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }

    public static GetIdentityUserByUserIdResponse MapFromModel(ApplicationIdentityUser user)
    {
        return new GetIdentityUserByUserIdResponse
        {
            Id = user.Id,
            Email = user.Email!,
            UserName = user.UserName!
        };
    }
}