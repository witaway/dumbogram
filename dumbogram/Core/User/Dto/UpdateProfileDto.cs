using FluentValidation;

namespace Dumbogram.Core.User.Dto;

public class UpdateProfileDto
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public Guid? AvatarMediaId { get; set; }
}

public class UpdateProfileDtoValidator : AbstractValidator<UpdateProfileDto>
{
    public UpdateProfileDtoValidator()
    {
        // Name is null or Name.Length in [1; 255]
        RuleFor(profile => profile.Name).Length(1, 255)
            .When(profile => profile.Name != null);

        // Description is null or Description.Length in [1; 255]
        RuleFor(profile => profile.Description).Length(1, 255)
            .When(profile => profile.Description != null);
    }
}