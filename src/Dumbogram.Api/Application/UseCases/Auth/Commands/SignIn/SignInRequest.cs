using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.Auth.Commands.SignIn;

public record SignInRequest : IRequest<Result<SignInResponse>>
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string Password { get; set; } = null!;
}