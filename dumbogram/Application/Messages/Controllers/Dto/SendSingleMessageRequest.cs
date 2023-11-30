using FluentValidation;

namespace Dumbogram.Application.Messages.Controllers.Dto;

public class SendSingleMessageRequest
{
    public string? Content { get; set; }
    public int? ReplyTo { get; set; }
}

public class SendSingleMessageRequestValidator : AbstractValidator<SendSingleMessageRequest>
{
    public SendSingleMessageRequestValidator()
    {
        RuleFor(request => request.Content).NotNull().NotEmpty().MaximumLength(2048);
    }
}