using Dumbogram.Api.Common.Classes;
using Dumbogram.Api.Infrastructure.Files.StorageWriter;
using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.FilesGroups.Commands.UploadFilesToFilesGroup;

public record UploadFilesToFilesGroupRequest(
    Guid GroupId,
    IAsyncEnumerable<FileContainer> FileContainers
) : IRequest<Result<Results<string, FileRecord>>>;