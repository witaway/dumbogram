using Dumbogram.Common.Errors;

namespace Dumbogram.Core.Chats.Errors;

public class UserAlreadyJoinedToChatError : ApplicationError
{
    public UserAlreadyJoinedToChatError()
        : base(nameof(UserAlreadyJoinedToChatError))
    {
    }
}

public class UserAlreadyBannedInChatError : ApplicationError
{
    public UserAlreadyBannedInChatError()
        : base(nameof(UserAlreadyBannedInChatError))
    {
    }
}

public class UserAlreadyLeftFromChatError : ApplicationError
{
    public UserAlreadyLeftFromChatError()
        : base(nameof(UserAlreadyLeftFromChatError))
    {
    }
}

public class UserBannedInChatError : ApplicationError
{
    public UserBannedInChatError()
        : base(nameof(UserBannedInChatError))
    {
    }
}