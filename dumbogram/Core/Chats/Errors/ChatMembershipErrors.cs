using Dumbogram.Common.Errors;

namespace Dumbogram.Core.Chats.Errors;

public class UserAlreadyJoinedToChat : BaseApplicationError
{
    public UserAlreadyJoinedToChat(string message)
        : base(nameof(UserAlreadyJoinedToChat), message)
    {
    }
}

public class UserAlreadyBannedInChat : BaseApplicationError
{
    public UserAlreadyBannedInChat(string message)
        : base(nameof(UserAlreadyBannedInChat), message)
    {
    }
}

public class UserAlreadyLeavedFromChat : BaseApplicationError
{
    public UserAlreadyLeavedFromChat(string message)
        : base(nameof(UserAlreadyLeavedFromChat), message)
    {
    }
}

public class UserBannedInChat : BaseApplicationError
{
    public UserBannedInChat(string message)
        : base(nameof(UserBannedInChat), message)
    {
    }
}