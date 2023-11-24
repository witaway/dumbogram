﻿using Dumbogram.Application.Chats.Models;

namespace Dumbogram.Application.Chats.Dto;

public class ReadMultipleChatsShortInfoResponseDto : List<ReadSingleChatShortInfoResponseDto>
{
    public ReadMultipleChatsShortInfoResponseDto(IEnumerable<Chat> chats)
    {
        AddRange(chats.Select(
            chat => new ReadSingleChatShortInfoResponseDto(chat)
        ));
    }
}