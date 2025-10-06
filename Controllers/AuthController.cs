using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Models;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Services;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend_ASP.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRegisterRequest body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        UserResponse user = await authService.RegisterUserAsync(body);

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthLoginRequest body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        string token = await authService.LoginUserAsync(body);

        Response.Cookies.Append(AppConstants.AuthTokenName, token, new CookieOptions
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
            await authService.LogoutUserAsync(token);
        }

        return Ok();
    }
}

