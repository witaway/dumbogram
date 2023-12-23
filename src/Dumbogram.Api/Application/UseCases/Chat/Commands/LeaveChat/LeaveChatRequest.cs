using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.Chat.Commands.LeaveChat;

public record LeaveChatRequest(Guid ChatId) : IRequest<Result>;