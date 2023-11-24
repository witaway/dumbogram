using Dumbogram.Application.Chats.Models;
using FluentValidation;

namespace Dumbogram.Application.Chats.Dto;

public class ApplyRightsRequestDto : List<string>
{
    public List<MembershipRight> ConvertToRightsList()
    {
        MembershipRight RightNameToRight(string rightName)
        {
            return (MembershipRight)Enum.Parse(typeof(MembershipRight), rightName, true);
        }

        return this.Select(RightNameToRight).ToList();
    }
}

public class ApplyRightsRequestDtoValidator : AbstractValidator<ApplyRightsRequestDto>
{
    public ApplyRightsRequestDtoValidator()
    {
        bool RightNameMatchesExistingRight(string rightName)
        {
            return Enum.TryParse(rightName, true, out MembershipRight _);
        }

        // Check if every right name in list is a correct enum value
        RuleForEach(dto => dto).Must(RightNameMatchesExistingRight);
    }
}