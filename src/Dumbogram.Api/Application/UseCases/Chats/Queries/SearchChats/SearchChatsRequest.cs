using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.Chats.Queries.SearchChats;

public record SearchChatsRequest : IRequest<Result<IEnumerable<Persistence.Context.Application.Entities.Chats.Chat>>>;