using Dumbogram.Api.Persistence.Context.Application.Entities.Users;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.ChatMembers.Queries.GetChatMembersBanned;

public record GetChatMembersBannedRequest(Guid ChatId) : IRequest<Result<IEnumerable<UserProfile>>>;