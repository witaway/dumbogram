using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using FluentValidation;

namespace Dumbogram.Api.Application.UseCases.ChatMember.Commands.ApplyChatMemberRights;

public class ApplyChatMemberRightsRequestValidator : AbstractValidator<ApplyChatMemberRightsRequest>
{
    public ApplyChatMemberRightsRequestValidator()
    {
        bool RightNameMatchesExistingRight(string rightName)
        {
            return Enum.TryParse(rightName, true, out MembershipRight _);
        }

        // Check if every right name in list is a correct enum value
        RuleForEach(dto => dto.RightsNames)
            .Must(RightNameMatchesExistingRight);
    }
}