namespace Dumbogram.Models.Messages;

/// <summary>
///     Class representing system message details.
///     Contains inner classes deriving from abstract SystemMessageDetails with more specific content
///     Name of child classes should strictly match corresponding SystemMessageType
///     Usage: new SystemMessageDetails.ChatDescriptionEdited() {}
/// </summary>
public class SystemMessageDetails
{
    public string? NewDescription { get; set; }
    public string? NewTitle { get; set; }

    public static SystemMessageDetails ChatDescriptionEdited(string newDescription)
    {
        return new SystemMessageDetails
        {
            NewDescription = newDescription
        };
    }

    public static SystemMessageDetails ChatTitleEdited(string newTitle)
    {
        return new SystemMessageDetails
        {
            NewTitle = newTitle
        };
    }
}