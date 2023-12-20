﻿using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;

namespace Dumbogram.Api.Application.Messages.Controllers.Dto;

public class ReadManyMessagesResponse : List<ReadSingleMessageResponse>
{
    public ReadManyMessagesResponse(IEnumerable<Message> messages)
    {
        AddRange(messages.Select(message => new ReadSingleMessageResponse(message)));
    }
}