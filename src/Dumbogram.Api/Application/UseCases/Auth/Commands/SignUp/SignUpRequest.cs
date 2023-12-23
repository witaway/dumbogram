using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.Auth.Commands.SignUp;

public class SignUpRequest : IRequest<Result>
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}