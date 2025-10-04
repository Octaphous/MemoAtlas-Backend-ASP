using MemoAtlas_Backend_ASP.Filters;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Services;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend_ASP.Controllers
{
    [Route("api/[controller]")]
    [AuthRequired]
    [ApiController]
    public class UserController(IUserContext userContext) : ControllerBase
    {
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            UserData user = userContext.CurrentUser!;
            return Ok(user);
        }
    }
}
