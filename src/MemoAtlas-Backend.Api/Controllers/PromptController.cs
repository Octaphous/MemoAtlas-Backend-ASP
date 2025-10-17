using MemoAtlas_Backend.Api.Filters;
using MemoAtlas_Backend.Api.Mappers;
using MemoAtlas_Backend.Api.Models.DTOs.Requests;
using MemoAtlas_Backend.Api.Models.DTOs.Responses;
using MemoAtlas_Backend.Api.Models.Entities;
using MemoAtlas_Backend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MemoAtlas_Backend.Api.Controllers;

[Route("api/prompts")]
[AuthRequired]
[ApiController]
public class PromptController(IUserContext auth, IPromptService promptService, IPromptStatsService promptStatsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllPrompts()
    {
        IEnumerable<Prompt> prompts = await promptService.GetAllPromptsAsync(auth.GetRequiredUser());
        return Ok(prompts.Select(PromptMapper.ToDTO));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPrompt(int id)
    {
        Prompt prompt = await promptService.GetPromptAsync(auth.GetRequiredUser(), id);
        return Ok(PromptMapper.ToPromptWithMemosDTO(prompt));
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetPromptStats([FromQuery] PromptStatsFilterRequest filter)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        PromptStatsAllDTO stats = await promptStatsService.GetAllPromptStatsAsync(auth.GetRequiredUser(), filter);
        return Ok(stats);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrompt([FromBody] PromptCreateRequest body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        Prompt prompt = await promptService.CreatePromptAsync(auth.GetRequiredUser(), body);
        return Ok(PromptMapper.ToDTO(prompt));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdatePrompt(int id, [FromBody] PromptUpdateRequest body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        Prompt prompt = await promptService.UpdatePromptAsync(auth.GetRequiredUser(), id, body);
        return Ok(PromptMapper.ToDTO(prompt));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrompt(int id)
    {
        await promptService.DeletePromptAsync(auth.GetRequiredUser(), id);
        return Ok();
    }
}
