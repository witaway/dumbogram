﻿namespace Dumbogram.Api.Models.Messages;

public class UserMessageContent
{
    public string? Text { get; set; }
    public Guid? AttachedPhotosGroupId { get; set; }
}