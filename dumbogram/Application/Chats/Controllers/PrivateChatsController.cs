using Dumbogram.Common.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Application.Chats.Controllers;

[Authorize]
[Route("/api/chats/private")]
[ApiController]
public class PrivateChatsController : ApplicationController
{
    // TODO: Implement private chats logic
    // (Should be possible to join or view information by private link (maybe JWT?)
    // But there's NO WAY to get all the links for all possible chats
}