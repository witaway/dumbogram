using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.ChatMessage.Commands.SendChatMessage;

public record SendChatMessageRequest(Guid ChatId, SendChatMessageRequestContent MessageContent)
    : IRequest<Result<UserMessage>>;