using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.ChatMessage.Queries.GetChatMessage;

public record GetChatMessageRequest(Guid ChatId, int MessageId) : IRequest<Result<Message>>;