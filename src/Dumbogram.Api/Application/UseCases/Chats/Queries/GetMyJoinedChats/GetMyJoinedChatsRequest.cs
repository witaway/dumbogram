using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.Chats.Queries.GetMyJoinedChats;

public record GetMyJoinedChatsRequest : IRequest<Result<IEnumerable<Persistence.Context.Application.Entities.Chats.Chat>>>;