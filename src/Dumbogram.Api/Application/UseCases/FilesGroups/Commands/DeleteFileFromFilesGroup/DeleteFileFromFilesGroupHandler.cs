using Dumbogram.Api.Domain.Services.Users;
using Dumbogram.Api.Infrastructure.Files;
using Dumbogram.Api.Infrastructure.Files.Errors;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.FilesGroups.Commands.DeleteFileFromFilesGroup;

public class DeleteFileFromFilesGroupHandler(
    UserResolverService userResolverService,
    FilesGroupService filesGroupService
) : IRequestHandler<DeleteFileFromFilesGroupRequest, Result>
{
    public async Task<Result> Handle(DeleteFileFromFilesGroupRequest request, CancellationToken cancellationToken)
    {
        var currentUser = await userResolverService.GetApplicationUser();
        var groupId = request.GroupId;
        var fileId = request.FileId;

        var groupResult = await filesGroupService.RequestOwnedFilesGroupById(currentUser, groupId);
        if (groupResult.IsFailed) return Result.Fail(groupResult.Errors);

        var group = groupResult.Value;
        var file = group.Files.SingleOrDefault(file => file.Id == fileId);

        if (file == null) return Result.Fail(new FileNotExistError());

        await filesGroupService.RemoveFileFromFilesGroup(group, file);

        return Result.Ok();
    }
}