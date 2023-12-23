using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.ChatMessage.Commands.DeleteChatMessage;

public record DeleteChatMessageRequest(Guid ChatId, int MessageId) : IRequest<Result>;