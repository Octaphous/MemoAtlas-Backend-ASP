using MemoAtlas_Backend.Api.Exceptions;
using MemoAtlas_Backend.Api.Models.Configurations;
using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MemoAtlas_Backend.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IUserService userService, ISessionService sessionService, IOptions<AuthOptions> authOptions) : ControllerBase
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

        Response.Cookies.Append(authOptions.Value.SessionTokenName, session.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(authOptions.Value.SessionDurationHours)
        });

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (Request.Cookies.TryGetValue(authOptions.Value.SessionTokenName, out string? token))
        {
            Response.Cookies.Delete(authOptions.Value.SessionTokenName);
            try
            {
                await sessionService.DeleteSessionAsync(token);
            }
            catch (InvalidResourceException) { }
        }

        return Ok();
    }
}

