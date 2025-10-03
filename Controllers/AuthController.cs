using MemoAtlas_Backend_ASP.Data;
using MemoAtlas_Backend_ASP.Extensions;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Services;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend_ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterBody body)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            UserDTO? user = await authService.RegisterUserAsync(body);
            if (user == null) return this.ErrorMsg("User registration failed");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginBody body)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string? token = await authService.LoginUserAsync(body);
            if (token == null) return Unauthorized("Invalid email or password");

            Response.Cookies.Append("SessionToken", token, new CookieOptions
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
            if (Request.Cookies.TryGetValue("SessionToken", out string? token))
            {
                Response.Cookies.Delete("SessionToken");
                await authService.LogoutUserAsync(token);
            }

            return Ok();
        }
    }
}
