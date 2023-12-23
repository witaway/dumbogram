using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.ChatMember.Queries.GetChatMemberRights;

public record GetChatMemberRightsRequest(Guid ChatId, Guid MemberId) : IRequest<Result<IEnumerable<MembershipRight>>>;