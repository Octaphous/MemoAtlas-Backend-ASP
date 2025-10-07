using System.Text.Json;
using MemoAtlas_Backend_ASP.Filters;
using MemoAtlas_Backend_ASP.Mappers;
using MemoAtlas_Backend_ASP.Models.DTOs;
using MemoAtlas_Backend_ASP.Models.DTOs.Requests;
using MemoAtlas_Backend_ASP.Models.DTOs.Responses;
using MemoAtlas_Backend_ASP.Models.Entities;
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
        IEnumerable<MemoWithCountsDTO> memos = await memoService.ListAllMemosAsync(auth.GetRequiredUser());
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
