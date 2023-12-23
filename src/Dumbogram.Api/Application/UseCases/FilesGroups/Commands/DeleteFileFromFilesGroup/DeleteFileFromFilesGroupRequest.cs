using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.FilesGroups.Commands.DeleteFileFromFilesGroup;

public record DeleteFileFromFilesGroupRequest(Guid GroupId, Guid FileId) : IRequest<Result>;