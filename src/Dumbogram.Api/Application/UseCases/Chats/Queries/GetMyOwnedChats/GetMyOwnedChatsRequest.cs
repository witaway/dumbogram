using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.Chats.Queries.GetMyOwnedChats;

public record GetMyOwnedChatsRequest : IRequest<Result<IEnumerable<Persistence.Context.Application.Entities.Chats.Chat>>>;