using FluentValidation;

namespace Dumbogram.Api.Application.UseCases.ChatMessage.Commands.SendChatMessage;

public class SendChatMessageRequestValidator : AbstractValidator<SendChatMessageRequest>
{
    public SendChatMessageRequestValidator()
    {
        RuleFor(dto => dto);
    }
}