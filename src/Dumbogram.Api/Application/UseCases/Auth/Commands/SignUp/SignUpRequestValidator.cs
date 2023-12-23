using FluentValidation;

namespace Dumbogram.Api.Application.UseCases.Auth.Commands.SignUp;

public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(request => request.Email)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(request => request.Password)
            .NotEmpty()
            .MaximumLength(255);
    }
}