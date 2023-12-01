using FluentResults;

namespace Dumbogram.Models.Messages;

public class UserMessageContentBuilder
{
    public UserMessageContentBuilder(UserMessageContent? messageContent = null)
    {
        ResultUserMessageContent = messageContent ?? new UserMessageContent();
    }

    private UserMessageContent ResultUserMessageContent { get; }
    private IEnumerable<IError> Errors { get; } = new List<IError>();

    public UserMessageContentBuilder WithText(string text)
    {
        ResultUserMessageContent.Text = text;
        return this;
    }

    public Result<UserMessageContent> Build()
    {
        if (Errors.Any())
        {
            return Result.Fail(Errors);
        }

        return Result.Ok(ResultUserMessageContent);
    }
}