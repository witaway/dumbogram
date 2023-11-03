using FluentValidation;

namespace Dumbogram.Dto;

public class SignUpDto
{
    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public UpdateProfileDto? Profile { get; set; }
}

public class SignUpDtoValidator : AbstractValidator<SignUpDto>
{
    public SignUpDtoValidator()
    {
        // Password.Length in [1; 255]
        RuleFor(credentials => credentials.Username).NotEmpty().MaximumLength(255);

        // Email.Length in [1; 255]
        RuleFor(credentials => credentials.Email).NotEmpty().MaximumLength(255);

        // Password.Length in [1; 255]
        RuleFor(credentials => credentials.Password).NotEmpty().MaximumLength(255);
    }
}