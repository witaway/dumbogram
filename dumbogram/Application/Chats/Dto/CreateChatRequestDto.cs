using FluentValidation;

namespace Dumbogram.Application.Chats.Dto;

public class CreateChatRequestDto
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
}

public class CreateChatRequestDtoValidator : AbstractValidator<CreateChatRequestDto>
{
    public CreateChatRequestDtoValidator()
    {
        RuleFor(dto => dto.Title).NotEmpty().MaximumLength(64);
        RuleFor(dto => dto.Description).MaximumLength(256);
    }
}