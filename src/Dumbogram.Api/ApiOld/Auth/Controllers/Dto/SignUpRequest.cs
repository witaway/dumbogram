using Dumbogram.Api.ApiOld.Users.Controllers.Dto;
using FluentValidation;

namespace Dumbogram.Api.ApiOld.Auth.Controllers.Dto;

public class SignUpRequest
{
    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public UpdateProfileRequest? Profile { get; set; }
}

public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
        // Password.Length in [1; 255]
        RuleFor(credentials => credentials.Username).NotEmpty().MaximumLength(255);

        // Email.Length in [1; 255]
        RuleFor(credentials => credentials.Email).NotEmpty().MaximumLength(255);

        // Password.Length in [1; 255]
        RuleFor(credentials => credentials.Password).NotEmpty().MaximumLength(255);
    }
}