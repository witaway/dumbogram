using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.Chat.Commands.JoinChat;

public record JoinChatRequest(Guid ChatId) : IRequest<Result>;