namespace Dumbogram.Api.ApiOld.Auth.Controllers.Dto;

public class SignInResponse
{
    public required string Token { get; set; }
    public required DateTime Expiration { get; set; }
}