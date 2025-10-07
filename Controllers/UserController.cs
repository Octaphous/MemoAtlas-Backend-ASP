using MemoAtlas_Backend_ASP.Filters;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;

namespace MemoAtlas_Backend_ASP.Controllers;

[Route("api/me")]
[AuthRequired]
[ApiController]
public class UserController(IUserContext auth, IUserService userService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetCurrentUser()
    {
        return Ok(UserMapper.ToDTO(auth.GetRequiredUser()));
    }

    [HttpPost("enable-private")]
    public async Task<IActionResult> EnablePrivateMode([FromBody] UserEnablePrivateRequest body)
    {
        await userService.EnablePrivateModeAsync(auth.GetRequiredUser(), body);
        return NoContent();
    }

    [HttpPost("disable-private")]
    public async Task<IActionResult> DisablePrivateMode()
    {
        await userService.DisablePrivateModeAsync(auth.GetRequiredUser());
        return NoContent();
    }
}