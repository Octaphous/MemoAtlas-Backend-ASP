using MemoAtlas_Backend_ASP.Filters;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using MemoAtlas_Backend_ASP.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend_ASP.Controllers;

[Route("api/users")]
[AuthRequired]
[ApiController]
public class UserController(IUserContext userContext) : ControllerBase
{
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        User user = userContext.CurrentUser!;
        return Ok(UserMapper.ToResponse(user));
    }
}