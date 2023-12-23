using Dumbogram.Api.Domain.Services.Users;
using Dumbogram.Api.Infrastructure.Files;
using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.FilesGroups.Commands.CreateFilesGroup;

public class CreateFilesGroupHandler(
    UserResolverService userResolverService,
    FilesGroupService filesGroupService
) : IRequestHandler<CreateFilesGroupRequest, Result<FilesGroup>>
{
    public async Task<Result<FilesGroup>> Handle(
        CreateFilesGroupRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await userResolverService.GetApplicationUser();
        var filesGroupType = request.FilesGroupType;

        var group = await filesGroupService.CreateFilesGroup(currentUser, filesGroupType);

        return Result.Ok(group);
    }
}