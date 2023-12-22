using Dumbogram.Api.ApiOld.Files.Services;
using Dumbogram.Api.ApiOld.Messages.Controllers.Dto;
using Dumbogram.Api.ApiOld.Messages.Services.Errors;
using Dumbogram.Api.Persistence.Context.Application;
using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;
using Dumbogram.Api.Persistence.Context.Application.Entities.Users;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using FluentResults;

namespace Dumbogram.Api.ApiOld.Messages.Services;

public class MessageContentBuilderService
{
    private readonly FilesGroupService _filesGroupService;
    private ApplicationDbContext _dbContext;

    public MessageContentBuilderService(
        ApplicationDbContext dbContext,
        FilesGroupService filesGroupService
    )
    {
        _dbContext = dbContext;
        _filesGroupService = filesGroupService;
    }

    public async Task<Result<UserMessageContent>> BuildMessageContent(UserProfile sender,
        MessageContentRequest messageContentRequest)
    {
        var errors = new List<IError>();

        var content = new UserMessageContent();
        var propertiesSet = 0;

        if (messageContentRequest.Text != null)
        {
            var trimmedText = messageContentRequest.Text.Trim();
            if (trimmedText.Length != 0)
            {
                content.Text = messageContentRequest.Text;
                propertiesSet++;
            }
        }

        var attachedPhotosGroupId = messageContentRequest.AttachedPhotosGroupId;
        if (attachedPhotosGroupId is not null)
        {
            var attachedPhotosGroupResult = await _filesGroupService.RequestOwnedFilesGroupById(
                sender,
                (Guid)attachedPhotosGroupId
            );
            if (attachedPhotosGroupResult.IsFailed) return Result.Fail(attachedPhotosGroupResult.Errors);

            var attachedPhotosGroup = attachedPhotosGroupResult.Value;

            if (attachedPhotosGroup.GroupType != FilesGroupType.AttachedPhotos)
                return Result.Fail(new BadMessageContent());

            content.AttachedPhotosGroupId = (Guid)attachedPhotosGroupId;
            propertiesSet++;
        }
        else
        {
            content.AttachedPhotosGroupId = null;
        }

        if (propertiesSet == 0) return Result.Fail(new MessageCannotBeEmpty());

        return Result.Ok(content);
    }
}