using Dumbogram.Models.Chats;
using FluentValidation;

namespace Dumbogram.Application.Chats.Controllers.Dto;

public class UpdateChatInfoRequest
{
    public Guid? OwnerId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ChatVisibility { get; set; }

    public ChatVisibility ChatVisibilityToEnum(string chatVisibility)
    {
        return (ChatVisibility)Enum.Parse(typeof(ChatVisibility), chatVisibility, true);
    }
}

public class UpdateChatInfoRequestValidator : AbstractValidator<UpdateChatInfoRequest>
{
    public UpdateChatInfoRequestValidator()
    {
        bool ChatVisibilityMatchesEnumValue(string chatVisibility)
        {
            return Enum.TryParse(chatVisibility, true, out ChatVisibility _);
        }

        // At least one of parameters must be NOT null
        RuleFor(dto => dto)
            .Must(dto => dto.OwnerId != null || dto.Title != null ||
                         dto.Description != null || dto.ChatVisibility != null)
            .WithMessage("At least one of parameters OwnerId, Title, Description, ChatVisibility must be set");

        // Description property size limit
        When(dto => dto.Description != null, () =>
        {
            RuleFor(dto => dto.Description!)
                .MaximumLength(256);
        });

        // Title property size limit
        When(dto => dto.Title != null, () =>
        {
            RuleFor(dto => dto.Title)
                .MaximumLength(64);
        });

        // ChatVisibility property must match Enum
        When(dto => dto.ChatVisibility != null, () =>
        {
            RuleFor(dto => dto.ChatVisibility!)
                .Must(ChatVisibilityMatchesEnumValue)
                .WithMessage("ChatVisibility is not a correct value of Enum type");
        });
    }
}