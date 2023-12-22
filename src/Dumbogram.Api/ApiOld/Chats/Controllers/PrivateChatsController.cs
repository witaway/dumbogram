using Dumbogram.Api.Infrasctructure.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Api.ApiOld.Chats.Controllers;

[Authorize]
[Route("/api/chats/private", Name = "Private chats")]
[ApiController]
public class PrivateChatsController : ApplicationController
{
    // TODO: Implement private chats logic
    // (Should be possible to join or view information by private link (maybe JWT?)
    // But there's NO WAY to get all the links for all possible chats
}