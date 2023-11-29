using FluentValidation;

namespace Dumbogram.Application.Chats.Controllers.Dto;

public class CreateChatRequest
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
}

public class CreateChatRequestValidator : AbstractValidator<CreateChatRequest>
{
    public CreateChatRequestValidator()
    {
        RuleFor(dto => dto.Title).NotEmpty().MaximumLength(64);
        RuleFor(dto => dto.Description).MaximumLength(256);
    }
}