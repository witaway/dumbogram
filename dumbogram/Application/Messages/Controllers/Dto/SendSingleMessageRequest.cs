using Dumbogram.Models.Messages;
using FluentValidation;

namespace Dumbogram.Application.Messages.Controllers.Dto;

public class SendSingleMessageRequest : UserMessageContent
{
    public int? ReplyTo { get; set; }
}

public class SendSingleMessageRequestValidator : AbstractValidator<SendSingleMessageRequest>
{
    public SendSingleMessageRequestValidator()
    {
        RuleFor(request => request.Text).NotNull().NotEmpty().MaximumLength(2048);
    }
}