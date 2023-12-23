using Dumbogram.Api.Persistence.Context.Application.Entities.Users;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.ChatMembers.Queries.GetChatMembers;

public record GetChatMembersRequest(Guid ChatId) : IRequest<Result<IEnumerable<UserProfile>>>;