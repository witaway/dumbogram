namespace Dumbogram.Models.Messages.SystemMessages;

/// <summary>
///     Class representing system message details.
///     Contains inner classes deriving from abstract SystemMessageDetails with more specific content
///     Name of child classes should strictly match corresponding SystemMessageType
///     Usage: new SystemMessageDetails.ChatDescriptionEdited() {}
/// </summary>
public class SystemMessageDetails
{
    public class ChatDescriptionEdited : SystemMessageDetails
    {
        public required string NewDescription { get; set; }
    }

    public class ChatTitleEdited : SystemMessageDetails
    {
        public required string NewTitle { get; set; }
    }
}