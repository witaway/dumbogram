using FluentValidation;

namespace Dumbogram.Api.Application.UseCases.Chat.Commands.CreateChat;

public class CreateChatRequestValidator : AbstractValidator<CreateChatRequest>
{
    public CreateChatRequestValidator()
    {
        RuleFor(dto => dto.Title).NotEmpty().MaximumLength(64);
        RuleFor(dto => dto.Description).MaximumLength(256);
    }
}