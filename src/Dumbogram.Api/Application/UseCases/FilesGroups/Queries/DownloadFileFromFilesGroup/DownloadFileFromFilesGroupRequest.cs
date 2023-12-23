using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.FilesGroups.Queries.DownloadFileFromFilesGroup;

public record DownloadFileFromFilesGroupRequest(Guid GroupId, Guid FileId)
    : IRequest<Result<DownloadFileFromFilesGroupResult>>;