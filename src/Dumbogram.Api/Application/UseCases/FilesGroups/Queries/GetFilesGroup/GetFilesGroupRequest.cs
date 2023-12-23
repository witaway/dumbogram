using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.FilesGroups.Queries.GetFilesGroup;

public record GetFilesGroupRequest(Guid FilesGroupId) : IRequest<Result<FilesGroup>>;