using System.Text.Json;
using MemoAtlas_Backend_ASP.Filters;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Bodies;
using MemoAtlas_Backend_ASP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend_ASP.Controllers;

[Route("api/memos")]
[AuthRequired]
[ApiController]
public class MemoController(IUserContext auth, IMemoService memoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllMemos()
    {
        List<SummarizedMemoData> memos = await memoService.GetAllMemosAsync(auth.GetRequiredUser());
        return Ok(memos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMemo(int id)
    {
        MemoData memo = await memoService.GetMemoAsync(auth.GetRequiredUser(), id);
        return Ok(memo);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMemo([FromBody] MemoCreateBody body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        MemoData createdMemo = await memoService.CreateMemoAsync(auth.GetRequiredUser(), body);
        return Ok(createdMemo);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateMemo(int id, [FromBody] MemoUpdateBody memo)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await memoService.UpdateMemoAsync(auth.GetRequiredUser(), id, memo);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMemo(int id)
    {
        await memoService.DeleteMemoAsync(auth.GetRequiredUser(), id);
        return NoContent();
    }
}
