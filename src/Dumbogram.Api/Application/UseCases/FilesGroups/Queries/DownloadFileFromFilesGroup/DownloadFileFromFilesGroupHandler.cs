using Dumbogram.Api.Infrastructure.Files;
using Dumbogram.Api.Infrastructure.Files.Errors;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.FilesGroups.Queries.DownloadFileFromFilesGroup;

public class DownloadFileFromFilesGroupHandler(
    FilesGroupService filesGroupService,
    FileTransferService fileTransferService
) : IRequestHandler<DownloadFileFromFilesGroupRequest, Result<DownloadFileFromFilesGroupResult>>
{
    public async Task<Result<DownloadFileFromFilesGroupResult>> Handle(
        DownloadFileFromFilesGroupRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var groupId = request.GroupId;
        var fileId = request.FileId;

        // Retrieve files group
        var groupResult = await filesGroupService.RequestFilesGroupById(groupId);
        if (groupResult.IsFailed) return Result.Fail(groupResult.Errors);
        var group = groupResult.Value;

        // Retrieve file from files group
        var file = group
            .Files
            .SingleOrDefault(file => file.Id == fileId);

        if (file == null) return Result.Fail(new FileNotExistError());

        // Form result and send it
        var contentType = file.MimeType;
        var downloadName = file.OriginalFileName;
        var fileStream = fileTransferService.DownloadFile(file);

        var result = new DownloadFileFromFilesGroupResult(fileStream, downloadName, contentType);

        return Result.Ok(result);
    }
}