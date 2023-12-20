namespace Dumbogram.Api.Persistence.Context.Application.Enumerations;

public enum FilesGroupType
{
    /// <summary>
    ///     Avatars of user. Infinite quantity, no limit
    /// </summary>
    Avatars,

    /// <summary>
    ///     Photos attached to UserMessage. Maximum quantity: 10 photos in group/message
    /// </summary>
    AttachedPhotos,

    /// <summary>
    ///     Videos attached to UserMessage. Maximum quantity: 5 videos per group/message
    /// </summary>
    AttachedVideos,

    /// <summary>
    ///     Documents attached to UserMessage. Maximum quantity: 5 documents per group/message
    /// </summary>
    AttachedDocuments
}