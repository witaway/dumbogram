using Dumbogram.Models.Chats;
using FluentValidation;

namespace Dumbogram.Application.Chats.Controllers.Dto;

public class ApplyRightsRequest : List<string>
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

public class ApplyRightsRequestValidator : AbstractValidator<ApplyRightsRequest>
{
    public ApplyRightsRequestValidator()
    {
        bool RightNameMatchesExistingRight(string rightName)
        {
            return Enum.TryParse(rightName, true, out MembershipRight _);
        }

        // Check if every right name in list is a correct enum value
        RuleForEach(dto => dto).Must(RightNameMatchesExistingRight);
    }
}