using MemoAtlas_Backend_ASP.Filters;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend_ASP.Controllers
{
    [Route("api/memos")]
    [AuthRequired]
    [ApiController]
    public class MemoController(IUserContext userContext, IMemoService memoService) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllMemos()
        {
            UserData user = userContext.CurrentUser!;

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetMemo(int id)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMemo([FromBody] MemoCreateBody body)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            MemoData createdMemo = await memoService.CreateMemoAsync(body);

            return Ok(createdMemo);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateMemo(int id, [FromBody] MemoUpdateBody memo)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMemo(int id)
        {
            return Ok();
        }
    }
}