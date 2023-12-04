using Dumbogram.Application.Messages.Controllers.Dto;
using Dumbogram.Database;
using Dumbogram.Models.Users;
using FluentResults;

namespace Dumbogram.Models.Messages;

public class UserMessageContentBuilder
{
    private ApplicationDbContext _dbContext;
    private UserProfile sender;

    public UserMessageContentBuilder(ApplicationDbContext dbContext, UserProfile sender,
        MessageContentRequest messageContentRequest)
    {
        var content = new UserMessageContent();
        var errors = new List<IError>();

        if (messageContentRequest.Text != null)
        {
            content.Text = messageContentRequest.Text;
        }

        if (messageContentRequest.AttachedPhotosGroupId != null)
        {
        }
    }

    private UserMessageContent ResultUserMessageContent { get; }
    private IEnumerable<IError> Errors { get; } = new List<IError>();

    public Result<UserMessageContent> Build()
    {
        if (Errors.Any())
        {
            return Result.Fail(Errors);
        }

        return Result.Ok(ResultUserMessageContent);
    }
}