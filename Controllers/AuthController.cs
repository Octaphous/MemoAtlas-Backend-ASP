using MemoAtlas_Backend_ASP.Models;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.Entities;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend_ASP.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IUserService userService, ISessionService sessionService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateRequest body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        User user = await userService.CreateUserAsync(body);

        return Ok(UserMapper.ToDTO(user));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        User user = await userService.GetUserFromCredentialsAsync(body);
        Session session = await sessionService.CreateSessionAsync(user);

        Response.Cookies.Append(AppConstants.AuthTokenName, session.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (Request.Cookies.TryGetValue(AppConstants.AuthTokenName, out string? token))
        {
            Response.Cookies.Delete(AppConstants.AuthTokenName);
            await sessionService.DeleteSessionAsync(token);
        }

        return Ok();
    }
}

