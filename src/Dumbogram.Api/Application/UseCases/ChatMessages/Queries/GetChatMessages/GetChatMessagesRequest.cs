using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.ChatMessages.Queries.GetChatMessages;

public record GetChatMessagesRequest(Guid ChatId) : IRequest<Result<IEnumerable<Message>>>;