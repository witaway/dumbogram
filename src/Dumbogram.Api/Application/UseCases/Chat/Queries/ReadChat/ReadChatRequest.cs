using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.Chat.Queries.ReadChat;

public record ReadChatRequest(Guid ChatId) : IRequest<Result<Persistence.Context.Application.Entities.Chats.Chat>>;