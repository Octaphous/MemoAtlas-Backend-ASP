using MemoAtlas_Backend.Api.Filters;
using MemoAtlas_Backend.Api.Mappers;
using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend.Api.Controllers;

[Route("api/memos")]
[AuthRequired]
[ApiController]
public class MemoController(IUserContext auth, IMemoService memoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllMemos([FromQuery] MemoFilterRequest filter)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        IEnumerable<MemoWithCountsDTO> memos = await memoService.ListAllMemosAsync(auth.GetRequiredUser(), filter);
        return Ok(memos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMemo(int id)
    {
        Memo memo = await memoService.GetMemoAsync(auth.GetRequiredUser(), id);
        return Ok(MemoMapper.ToMemoWithTagsAndAnswersDTO(memo));
    }

    [HttpPost]
    public async Task<IActionResult> CreateMemo([FromBody] MemoCreateRequest body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        Memo createdMemo = await memoService.CreateMemoAsync(auth.GetRequiredUser(), body);
        return Ok(MemoMapper.ToMemoWithTagsAndAnswersDTO(createdMemo));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateMemo(int id, [FromBody] MemoUpdateRequest memo)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        Memo updatedMemo = await memoService.UpdateMemoAsync(auth.GetRequiredUser(), id, memo);
        return Ok(MemoMapper.ToMemoWithTagsAndAnswersDTO(updatedMemo));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMemo(int id)
    {
        await memoService.DeleteMemoAsync(auth.GetRequiredUser(), id);
        return NoContent();
    }
}
