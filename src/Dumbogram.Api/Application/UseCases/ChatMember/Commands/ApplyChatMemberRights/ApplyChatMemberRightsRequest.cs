using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.ChatMember.Commands.ApplyChatMemberRights;

public record ApplyChatMemberRightsRequest(Guid ChatId, Guid MemberId, List<string> RightsNames) : IRequest<Result>
{
    public List<MembershipRight> Rights => RightsNames.Select(RightNameToRight).ToList();

    private static MembershipRight RightNameToRight(string rightName)
    {
        return (MembershipRight)Enum.Parse(
            typeof(MembershipRight),
            rightName,
            true
        );
    }
}