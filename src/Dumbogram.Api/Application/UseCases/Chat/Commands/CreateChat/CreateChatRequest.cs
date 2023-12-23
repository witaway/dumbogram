using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.Chat.Commands.CreateChat;

public record CreateChatRequest : IRequest<Result<Persistence.Context.Application.Entities.Chats.Chat>>
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
}