using FluentValidation;

namespace Dumbogram.Application.Messages.Controllers.Dto;

public class MessageContentRequest
{
    public string? Text { get; set; }
    public Guid? AttachedPhotosGroupId { get; set; }
}

public class SendSingleMessageRequestValidator : AbstractValidator<MessageContentRequest>
{
    public SendSingleMessageRequestValidator()
    {
        RuleFor(request => request.Text).NotNull().NotEmpty().MaximumLength(2048);
    }
}