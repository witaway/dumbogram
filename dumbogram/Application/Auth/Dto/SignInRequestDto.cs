using FluentValidation;

namespace Dumbogram.Application.Auth.Dto;

public class SignInRequestDto
{
    public string? Username { get; set; }

    public string? Email { get; set; }

    public string Password { get; set; }
}

public class SignInRequestDtoValidator : AbstractValidator<SignInRequestDto>
{
    public SignInRequestDtoValidator()
    {
        RuleFor(credentials => credentials)
            .Must(credentials =>
                !string.IsNullOrEmpty(credentials.Username) || !string.IsNullOrEmpty(credentials.Email))
            .WithMessage("Username or Email must be set")
            .DependentRules(() =>
            {
                RuleFor(credentials => credentials.Username).Null()
                    .When(credentials => credentials.Email != null)
                    .WithMessage("Username must be null when Email has value");

                RuleFor(credentials => credentials.Email).Null()
                    .When(credentials => credentials.Username != null)
                    .WithMessage("Email must be null when Username has value");
            });

        // Password.Length in [1; 255] if it's set
        RuleFor(credentials => credentials.Username).NotEmpty().MaximumLength(255)
            .When(credentials => credentials.Username != null);

        // Email.Length in [1; 255] if it's set
        RuleFor(credentials => credentials.Email).NotEmpty().MaximumLength(255)
            .When(credentials => credentials.Email != null);

        // Password.Length in [1; 255]
        RuleFor(credentials => credentials.Password).NotEmpty().MaximumLength(255);
    }
}