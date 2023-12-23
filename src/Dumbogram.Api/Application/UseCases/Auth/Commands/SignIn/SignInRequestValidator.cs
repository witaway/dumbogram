using FluentValidation;

namespace Dumbogram.Api.Application.UseCases.Auth.Commands.SignIn;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(request => request)
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
        RuleFor(request => request.Username)
            .NotEmpty()
            .MaximumLength(255)
            .When(credentials => credentials.Username != null);

        // Email.Length in [1; 255] if it's set
        RuleFor(request => request.Email)
            .NotEmpty()
            .MaximumLength(255)
            .When(credentials => credentials.Email != null);

        // Password.Length in [1; 255]
        RuleFor(request => request.Password)
            .NotEmpty()
            .MaximumLength(255);
    }
}