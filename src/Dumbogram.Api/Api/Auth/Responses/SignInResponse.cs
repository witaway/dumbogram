namespace Dumbogram.Api.Api.Auth.Responses;

public class SignInResponse
{
    public required string Token { get; set; }
    public required DateTime Expiration { get; set; }
}