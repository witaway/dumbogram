using Dumbogram.Api.Domain.Services.Users;
using Dumbogram.Api.Infrastructure.Files;
using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.FilesGroups.Queries.GetFilesGroup;

public class GetFilesGroupHandler(
    UserResolverService userResolverService,
    FilesGroupService filesGroupService
) : IRequestHandler<GetFilesGroupRequest, Result<FilesGroup>>
{
    public async Task<Result<FilesGroup>> Handle(GetFilesGroupRequest request, CancellationToken cancellationToken)
    {
        var currentUser = await userResolverService.GetApplicationUser();
        var filesGroupId = request.FilesGroupId;

        var groupResult = await filesGroupService.RequestFilesGroupById(filesGroupId);
        if (groupResult.IsFailed) return Result.Fail(groupResult.Errors);
        var group = groupResult.Value;

        return Result.Ok(group);
    }
}