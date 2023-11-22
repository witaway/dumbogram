using Dumbogram.Common.Errors;

namespace Dumbogram.Core.Chats.Errors;

public class UserAlreadyJoinedToChatError : BaseApplicationError
{
    public UserAlreadyJoinedToChatError(string message)
        : base(nameof(UserAlreadyJoinedToChatError), message)
    {
    }
}

public class UserAlreadyBannedInChatError : BaseApplicationError
{
    public UserAlreadyBannedInChatError(string message)
        : base(nameof(UserAlreadyBannedInChatError), message)
    {
    }
}

public class UserAlreadyLeftFromChatError : BaseApplicationError
{
    public UserAlreadyLeftFromChatError(string message)
        : base(nameof(UserAlreadyLeftFromChatError), message)
    {
    }
}

public class UserBannedInChatError : BaseApplicationError
{
    public UserBannedInChatError(string message)
        : base(nameof(UserBannedInChatError), message)
    {
    }
}